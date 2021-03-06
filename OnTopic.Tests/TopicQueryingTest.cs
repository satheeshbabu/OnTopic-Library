﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnTopic.Data.Caching;
using OnTopic.Metadata;
using OnTopic.Querying;
using OnTopic.Repositories;
using OnTopic.TestDoubles;

namespace OnTopic.Tests {

  /*============================================================================================================================
  | CLASS: TOPIC QUERYING TEST
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides unit tests for the <see cref="TopicExtensions"/> class.
  /// </summary>
  [TestClass]
  public class TopicQueryingTest {

    /*==========================================================================================================================
    | PRIVATE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
    readonly                    ITopicRepository                _topicRepository;

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="TopicQueryingTest"/> with shared resources.
    /// </summary>
    /// <remarks>
    ///   This uses the <see cref="StubTopicRepository"/> to provide data, and then <see cref="CachedTopicRepository"/> to
    ///   manage the in-memory representation of the data. While this introduces some overhead to the tests, the latter is a
    ///   relatively lightweight façade to any <see cref="ITopicRepository"/>, and prevents the need to duplicate logic for
    ///   crawling the object graph.
    /// </remarks>
    public TopicQueryingTest() {
      _topicRepository = new CachedTopicRepository(new StubTopicRepository());
    }

    /*==========================================================================================================================
    | TEST: FIND ALL BY ATTRIBUTE: RETURNS CORRECT TOPICS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Looks for a deeply nested child topic using only the attribute value.
    /// </summary>
    [TestMethod]
    public void FindAllByAttribute_ReturnsCorrectTopics() {

      var parentTopic           = TopicFactory.Create("ParentTopic", "Page", 1);
      var childTopic            = TopicFactory.Create("ChildTopic", "Page", 5, parentTopic);
      var grandChildTopic       = TopicFactory.Create("GrandChildTopic", "Page", 20, childTopic);
      var grandNieceTopic       = TopicFactory.Create("GrandNieceTopic", "Page", 3, childTopic);
      var greatGrandChildTopic  = TopicFactory.Create("GreatGrandChildTopic", "Page", 7, grandChildTopic);

      grandChildTopic.Attributes.SetValue("Foo", "Baz");
      greatGrandChildTopic.Attributes.SetValue("Foo", "Bar");
      grandNieceTopic.Attributes.SetValue("Foo", "Bar");

      Assert.ReferenceEquals(parentTopic.FindAllByAttribute("Foo", "Bar").First(), grandNieceTopic);
      Assert.AreEqual<int>(2, parentTopic.FindAllByAttribute("Foo", "Bar").Count);
      Assert.ReferenceEquals(parentTopic.FindAllByAttribute("Foo", "Baz").First(), grandChildTopic);

    }

    /*==========================================================================================================================
    | TEST: FIND FIRST PARENT: RETURNS CORRECT TOPIC
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given a deeply nested <see cref="Topic"/>, returns the first parent <see cref="Topic"/> which satisfies a delegate.
    /// </summary>
    [TestMethod]
    public void FindFirstParent_ReturnsCorrectTopic() {

      var parentTopic           = TopicFactory.Create("ParentTopic", "Page", 1);
      var childTopic            = TopicFactory.Create("ChildTopic", "Page", 5, parentTopic);
      var grandChildTopic       = TopicFactory.Create("GrandChildTopic", "Page", 20, childTopic);
      var greatGrandChildTopic  = TopicFactory.Create("GreatGrandChildTopic", "Page", 7, grandChildTopic);

      var foundTopic            = greatGrandChildTopic.FindFirstParent(t => t.Id is 5);

      Assert.ReferenceEquals(childTopic, foundTopic);

    }

    /*==========================================================================================================================
    | TEST: GET ROOT TOPIC: RETURNS ROOT TOPIC
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given a deeply nested <see cref="Topic"/>, returns the root <see cref="Topic"/>.
    /// </summary>
    [TestMethod]
    public void GetRootTopic_ReturnsRootTopic() {

      var parentTopic           = TopicFactory.Create("ParentTopic", "Page", 1);
      var childTopic            = TopicFactory.Create("ChildTopic", "Page", 5, parentTopic);
      var grandChildTopic       = TopicFactory.Create("GrandChildTopic", "Page", 20, childTopic);
      var greatGrandChildTopic  = TopicFactory.Create("GreatGrandChildTopic", "Page", 7, grandChildTopic);

      var rootTopic             = greatGrandChildTopic.GetRootTopic();

      Assert.ReferenceEquals(parentTopic, rootTopic);

    }

    /*==========================================================================================================================
    | TEST: GET BY UNIQUE KEY: ROOT KEY: RETURNS ROOT TOPIC
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given a root <see cref="Topic"/>, returns the root <see cref="Topic"/>.
    /// </summary>
    [TestMethod]
    public void GetByUniqueKey_RootKey_ReturnsRootTopic() {

      var parentTopic           = TopicFactory.Create("ParentTopic", "Page", 1);
      _                         = TopicFactory.Create("ChildTopic", "Page", 2, parentTopic);

      var foundTopic = parentTopic.GetByUniqueKey("ParentTopic");

      Assert.IsNotNull(foundTopic);
      Assert.ReferenceEquals(parentTopic, foundTopic);

    }

    /*==========================================================================================================================
    | TEST: GET BY UNIQUE KEY: VALID KEY: RETURNS TOPIC
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given a deeply nested <see cref="Topic"/>, returns the expected <see cref="Topic"/>.
    /// </summary>
    [TestMethod]
    public void GetByUniqueKey_ValidKey_ReturnsTopic() {

      var parentTopic           = TopicFactory.Create("ParentTopic", "Page", 1);
      var childTopic            = TopicFactory.Create("ChildTopic", "Page", 5, parentTopic);
      var grandChildTopic       = TopicFactory.Create("GrandChildTopic", "Page", 20, childTopic);
      var greatGrandChildTopic1 = TopicFactory.Create("GreatGrandChildTopic1", "Page", 7, grandChildTopic);
      var greatGrandChildTopic2 = TopicFactory.Create("GreatGrandChildTopic2", "Page", 7, grandChildTopic);

      var foundTopic = greatGrandChildTopic1.GetByUniqueKey("ParentTopic:ChildTopic:GrandChildTopic:GreatGrandChildTopic2");

      Assert.ReferenceEquals(greatGrandChildTopic2, foundTopic);

    }

    /*==========================================================================================================================
    | TEST: GET BY UNIQUE KEY: INVALID KEY: RETURNS NULL
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given an invalid <c>UniqueKey</c>, the <see cref="TopicExtensions.GetByUniqueKey(Topic, String)"/> returns
    ///   <c>null</c>.
    /// </summary>
    [TestMethod]
    public void GetByUniqueKey_InvalidKey_ReturnsNull() {

      var parentTopic           = TopicFactory.Create("ParentTopic", "Page", 1);
      var childTopic            = TopicFactory.Create("ChildTopic", "Page", 5, parentTopic);
      var grandChildTopic       = TopicFactory.Create("GrandChildTopic", "Page", 20, childTopic);
      var greatGrandChildTopic  = TopicFactory.Create("GreatGrandChildTopic", "Page", 7, grandChildTopic);

      var foundTopic = greatGrandChildTopic.GetByUniqueKey("ParentTopic:ChildTopic:GrandChildTopic:GreatGrandChildTopic2");

      Assert.IsNull(foundTopic);

    }

    /*==========================================================================================================================
    | TEST: GET CONTENT TYPE: VALID CONTENT TYPE: RETURNS CONTENT TYPE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given a deeply nested <see cref="Topic"/>, returns the expected <see cref="ContentTypeDescriptor"/>.
    /// </summary>
    [TestMethod]
    public void GetContentType_ValidContentType_ReturnsContentType() {

      var topic                 = _topicRepository.Load(11111);
      var contentTypeDescriptor = topic.GetContentTypeDescriptor();

      Assert.IsNotNull(contentTypeDescriptor);
      Assert.AreEqual<string>("Page", contentTypeDescriptor.Key);

    }

    /*==========================================================================================================================
    | TEST: GET CONTENT TYPE: INVALID CONTENT TYPE: RETURNS NULL
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given an invalid <see cref="ContentTypeDescriptor"/>, the <see cref="TopicExtensions.GetContentTypeDescriptor(Topic)"
    ///   /> returns <c>null</c>.
    /// </summary>
    [TestMethod]
    public void GetContentType_InvalidContentType_ReturnsNull() {

      var parentTopic           = _topicRepository.Load(11111);
      var topic                 = TopicFactory.Create("Test", "NonExistent", parentTopic);
      var contentTypeDescriptor = topic.GetContentTypeDescriptor();

      Assert.IsNull(contentTypeDescriptor);

    }

  } //Class
} //Namespace