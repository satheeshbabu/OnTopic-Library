﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Reflection;
using OnTopic.Attributes;
using OnTopic.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OnTopic.Tests {

  /*============================================================================================================================
  | CLASS: ATTRIBUTE VALUE COLLECTION TEST
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides unit tests for the <see cref="AttributeValueCollection"/> class.
  /// </summary>
  [TestClass]
  public class AttributeValueCollectionTest {

    /*==========================================================================================================================
    | TEST: GET VALUE: CORRECT VALUE: IS RETURNED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates a new topic and ensures that the key can be returned as an attribute.
    /// </summary>
    [TestMethod]
    public void GetValue_CorrectValue_IsReturned() {
      var topic = TopicFactory.Create("Test", "Container");
      Assert.AreEqual<string>("Test", topic.Attributes.GetValue("Key"));
    }

    /*==========================================================================================================================
    | TEST: GET VALUE: MISSING VALUE: RETURNS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Creates a new topic and requests an invalid attribute; ensures falls back to the default.
    /// </summary>
    [TestMethod]
    public void GetValue_MissingValue_ReturnsDefault() {
      var topic = TopicFactory.Create("Test", "Container");
      Assert.AreEqual<string>("Foo", topic.Attributes.GetValue("InvalidAttribute", "Foo"));
    }

    /*==========================================================================================================================
    | TEST: GET INTEGER: CORRECT VALUE: IS RETURNED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that integer values can be set and retrieved as expected.
    /// </summary>
    [TestMethod]
    public void GetInteger_CorrectValue_IsReturned() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetInteger("Number1", 1);

      Assert.AreEqual<int>(1, topic.Attributes.GetInteger("Number1", 5));

    }

    /*==========================================================================================================================
    | TEST: GET INTEGER: INCORRECT VALUE: RETURNS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that invalid values return the default.
    /// </summary>
    [TestMethod]
    public void GetInteger_IncorrectValue_ReturnsDefault() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Number3", "Invalid");

      Assert.AreEqual<int>(5, topic.Attributes.GetInteger("Number3", 5));

    }

    /*==========================================================================================================================
    | TEST: GET INTEGER: INCORRECT KEY: RETURNS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that incorrect key names return the default.
    /// </summary>
    [TestMethod]
    public void GetInteger_IncorrectKey_ReturnsDefault() {

      var topic = TopicFactory.Create("Test", "Container");

      Assert.AreEqual<int>(5, topic.Attributes.GetInteger("InvalidKey", 5));

    }

    /*==========================================================================================================================
    | TEST: GET DATETIME: CORRECT VALUE: IS RETURNED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that integer values can be set and retrieved as expected.
    /// </summary>
    [TestMethod]
    public void GetDateTime_CorrectValue_IsReturned() {

      var topic                 = TopicFactory.Create("Test", "Container");
      var dateTime1             = new DateTime(1976, 10, 15);

      topic.Attributes.SetDateTime("DateTime1", dateTime1);

      Assert.AreEqual<DateTime>(dateTime1, topic.Attributes.GetDateTime("DateTime1", DateTime.MinValue));

    }

    /*==========================================================================================================================
    | TEST: GET DATETIME: INCORRECT VALUE: RETURNS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that invalid values return the default.
    /// </summary>
    [TestMethod]
    public void GetDateTime_IncorrectValue_ReturnsDefault() {

      var topic                 = TopicFactory.Create("Test", "Container");
      var dateTime1             = new DateTime(1976, 10, 15);
      var dateTime2             = new DateTime(1981, 06, 03);

      topic.Attributes.SetDateTime("DateTime2", dateTime2);

      Assert.AreEqual<DateTime>(dateTime1, topic.Attributes.GetDateTime("DateTime3", dateTime1));

    }

    /*==========================================================================================================================
    | TEST: GET DATETIME: INCORRECT KEY: RETURNS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that incorrect key names return the default.
    /// </summary>
    [TestMethod]
    public void GetDateTime_IncorrectKey_ReturnsDefault() {

      var topic                 = TopicFactory.Create("Test", "Container");
      var dateTime1             = new DateTime(1976, 10, 15);
      var dateTime2             = new DateTime(1981, 06, 03);

      topic.Attributes.SetDateTime("DateTime2", dateTime2);

      Assert.AreEqual<DateTime>(dateTime1, topic.Attributes.GetDateTime("DateTime3", dateTime1));

    }

    /*==========================================================================================================================
    | TEST: GET BOOLEAN: CORRECT VALUE: IS RETURNED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that boolean values can be set and retrieved as expected.
    /// </summary>
    [TestMethod]
    public void GetBoolean_CorrectValue_IsReturned() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetBoolean("IsValue1", true);
      topic.Attributes.SetBoolean("IsValue2", false);

      Assert.IsTrue(topic.Attributes.GetBoolean("IsValue1", false));
      Assert.IsFalse(topic.Attributes.GetBoolean("IsValue2", true));

    }

    /*==========================================================================================================================
    | TEST: GET BOOLEAN: INCORRECT VALUE: RETURN DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that invalid values return the default.
    /// </summary>
    [TestMethod]
    public void GetBoolean_IncorrectValue_ReturnDefault() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("IsValue", "Invalid");

      Assert.IsTrue(topic.Attributes.GetBoolean("IsValue", true));
      Assert.IsFalse(topic.Attributes.GetBoolean("IsValue", false));

    }

    /*==========================================================================================================================
    | TEST: GET BOOLEAN: INCORRECT KEY: RETURN DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that incorrect key names return the default.
    /// </summary>
    [TestMethod]
    public void GetBoolean_IncorrectKey_ReturnDefault() {

      var topic = TopicFactory.Create("Test", "Container");

      Assert.IsTrue(topic.Attributes.GetBoolean("InvalidKey", true));
      Assert.IsFalse(topic.Attributes.GetBoolean("InvalidKey", false));

    }

    /*==========================================================================================================================
    | TEST: SET VALUE: CORRECT VALUE: IS RETURNED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets a custom attribute on a topic and ensures it can be retrieved.
    /// </summary>
    [TestMethod]
    public void SetValue_CorrectValue_IsReturned() {
      var topic = TopicFactory.Create("Test", "Container");
      topic.Attributes.SetValue("Foo", "Bar");
      Assert.AreEqual<string>("Bar", topic.Attributes.GetValue("Foo"));
    }

    /*==========================================================================================================================
    | TEST: SET VALUE: VALUE CHANGED: IS DIRTY?
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Modifies the value of a custom attribute on a topic and ensures it is marked as IsDirty.
    /// </summary>
    [TestMethod]
    public void SetValue_ValueChanged_IsDirty() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Foo", "Bar", false);
      topic.Attributes.SetValue("Foo", "Baz");

      Assert.IsTrue(topic.Attributes["Foo"].IsDirty);

    }

    /*==========================================================================================================================
    | TEST: SET VALUE: VALUE UNCHANGED: IS NOT DIRTY?
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets the value of a custom attribute to the existing value and ensures it is <i>not</i> marked as IsDirty.
    /// </summary>
    [TestMethod]
    public void SetValue_ValueUnchanged_IsNotDirty() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Fah", "Bar", false);
      topic.Attributes.SetValue("Fah", "Bar");

      Assert.IsFalse(topic.Attributes["Fah"].IsDirty);

    }

    /*==========================================================================================================================
    | TEST: SET VALUE: INVALID VALUE: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Attempts to violate the business logic by bypassing the property setter; ensures that business logic is enforced.
    /// </summary>
    [TestMethod]
    [ExpectedException(
      typeof(TargetInvocationException),
      "The topic allowed a key to be set via a back door, without routing it through the Key property."
    )]
    public void SetValue_InvalidValue_ThrowsException() {
      var topic = TopicFactory.Create("Test", "Container");
      topic.Attributes.SetValue("Key", "# ?");
    }

    /*==========================================================================================================================
    | TEST: ADD: VALID ATTRIBUTE VALUE: IS RETURNED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets a custom attribute on a topic by directly adding an <see cref="AttributeValue"/> instance; ensures it can be
    ///   retrieved.
    /// </summary>
    [TestMethod]
    public void Add_ValidAttributeValue_IsReturned() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.Remove("Key");
      topic.Attributes.Add(new AttributeValue("Key", "NewKey", false));

      Assert.AreEqual<string>("NewKey", topic.Key);

    }

    /*==========================================================================================================================
    | TEST: SET VALUE: INSERT INVALID ATTRIBUTE VALUE: THROWS EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Attempts to violate the business logic by bypassing SetValue() entirely; ensures that business logic is enforced.
    /// </summary>
    [TestMethod]
    [ExpectedException(
      typeof(TargetInvocationException),
      "The topic allowed a key to be set via a back door, without routing it through the Key property."
    )]
    public void Add_InvalidAttributeValue_ThrowsException() {
      var topic = TopicFactory.Create("Test", "Container");
      topic.Attributes.Remove("Key");
      topic.Attributes.Add(new AttributeValue("Key", "# ?"));
    }

    /*==========================================================================================================================
    | TEST: GET VALUE: INHERIT FROM PARENT: RETURNS PARENT VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Sets an attribute on the parent of a topic and ensures it can be retrieved using inheritance.
    /// </summary>
    [TestMethod]
    public void GetValue_InheritFromParent_ReturnsParentValue() {

      var topics = new Topic[8];

      for (var i = 0; i <= 7; i++) {
        var topic = TopicFactory.Create("Topic" + i, "Container");
        if (i > 0) topic.Parent = topics[i - 1];
        topics[i] = topic;
      }

      topics[0].Attributes.SetValue("Foo", "Bar");

      Assert.IsNull(topics[4].Attributes.GetValue("Foo", null));
      Assert.AreEqual<string>("Bar", topics[7].Attributes.GetValue("Foo", true));
      Assert.AreNotEqual<string>("Bar", topics[7].Attributes.GetValue("Foo", false));

    }

    /*==========================================================================================================================
    | TEST: GET VALUE: INHERIT FROM DERIVED: RETURNS DERIVED VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a long tree of derives topics, and ensures that the derived value is returned.
    /// </summary>
    [TestMethod]
    public void GetValue_InheritFromDerived_ReturnsDerivedValue() {

      var topics = new Topic[5];

      for (var i = 0; i <= 4; i++) {
        var topic = TopicFactory.Create("Topic" + i, "Container");
        if (i > 0) topics[i - 1].DerivedTopic = topic;
        topics[i] = topic;
      }

      topics[4].Attributes.SetValue("Foo", "Bar");

      Assert.AreEqual<string>("Bar", topics[0].Attributes.GetValue("Foo", null, true, true));

    }

    /*==========================================================================================================================
    | TEST: GET VALUE: EXCEEDS MAX HOPS: RETURNS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Establishes a long tree of derives topics, and ensures that inheritance will pursue no more than five hops.
    /// </summary>
    [TestMethod]
    public void GetValue_ExceedsMaxHops_ReturnsDefault() {

      var topics = new Topic[8];

      for (var i = 0; i <= 7; i++) {
        var topic = TopicFactory.Create("Topic" + i, "Container");
        if (i > 0) topics[i - 1].DerivedTopic = topic;
        topics[i] = topic;
      }

      topics[7].Attributes.SetValue("Foo", "Bar");

      Assert.IsNull(topics[0].Attributes.GetValue("Foo", null, true, true));

    }

  } //Class
} //Namespace