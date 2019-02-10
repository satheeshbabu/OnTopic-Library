﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using Ignia.Topics.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ignia.Topics.AspNetCore.Mvc {

  /*============================================================================================================================
  | CLASS: SERVICE COLLECTION EXTENSIONS
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides extension methods for configuring the OnTopic library with ASP.NET Core MVC.
  /// </summary>
  public static class ServiceCollectionExtensions {

    /*==========================================================================================================================
    | EXTENSION: ADD TOPIC SUPPORT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Configures the Razor engine to include OnTopic view locations via the <see cref="TopicViewLocationExpander"/>.
    /// </summary>
    public static IMvcBuilder AddTopicSupport(this IMvcBuilder services) {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires<ArgumentNullException>(services != null, nameof(services));

      /*------------------------------------------------------------------------------------------------------------------------
      | Register services
      \-----------------------------------------------------------------------------------------------------------------------*/
      services.Services.TryAddSingleton<IActionResultExecutor<TopicViewResult>, TopicViewResultExecutor>();

      /*------------------------------------------------------------------------------------------------------------------------
      | Configure services
      \-----------------------------------------------------------------------------------------------------------------------*/
      services.AddRazorOptions(o => {
        o.ViewLocationExpanders.Add(new TopicViewLocationExpander());
      });

      /*------------------------------------------------------------------------------------------------------------------------
      | Return services for fluent API
      \-----------------------------------------------------------------------------------------------------------------------*/
      return services;
    }

  } //Class
} //Namespace