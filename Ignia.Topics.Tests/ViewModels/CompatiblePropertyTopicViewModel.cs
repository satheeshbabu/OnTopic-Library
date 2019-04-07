﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ignia.Topics.Metadata;
using Ignia.Topics.ViewModels;

namespace Ignia.Topics.Tests.ViewModels {

  /*============================================================================================================================
  | VIEW MODEL: COMPATIBLE PROPERTY TOPIC
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides a strongly-typed data transfer object for testing views with a property that maps to a source object, but isn't
  ///   otherwise mapped by any of the conversion functions.
  /// </summary>
  /// <remarks>
  ///   This is a sample class intended for test purposes only; it is not designed for use in a production environment.
  /// </remarks>
  public class CompatiblePropertyTopicViewModel : TopicViewModel {

    public ModelType ModelType { get; set; }
    public IDictionary<string, string> Configuration { get; set; }

    } //Class
  } //Namespace