﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignia.Topics.Mapping;
using Ignia.Topics.ViewModels;

namespace Ignia.Topics.Tests.ViewModels {

  /*============================================================================================================================
  | VIEW MODEL: SAMPLE TOPIC
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides a strongly-typed data transfer object for testing basic mapping rules, including scalar and collection-based
  ///   properties.
  /// </summary>
  /// <remarks>
  ///   This is a sample class intended for test purposes only; it is not designed for use in a production environment.
  /// </remarks>
  public class SampleTopicViewModel : PageTopicViewModel {

    public string Property { get; set; }

    [Inherit]
    public string InheritedProperty { get; set; }

    [AttributeKey("Property")]
    public string PropertyAlias { get; set; }

    [Recurse(Relationships.Relationships)]
    public TopicViewModelCollection<PageTopicViewModel> Children { get; set; }

    [Recurse(Relationships.Children)]
    public TopicViewModelCollection<PageTopicViewModel> Cousins { get; set; }

    public TopicViewModelCollection<PageTopicViewModel> Categories { get; set; }

    public Collection<Topic> Related { get; set; }

    [Relationship("AmbiguousRelationship", RelationshipType.IncomingRelationship)]
    public TopicViewModelCollection<TopicViewModel> RelationshipAlias { get; set; } 

  } //Class

} //Namespace