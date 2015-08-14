/*==============================================================================================================================
| Author        Katherine Trunkey, Ignia LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System.Configuration;

namespace Ignia.Topics.Configuration {

  /*============================================================================================================================
  | CLASS: TOPICS SECTION
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides a customized version of the <see cref="ConfigurationSection"/> class in order to permit configuration of the
  ///   Topics data management classes via the application's web.config.
  /// </summary>
  public class TopicsSection : ConfigurationSection {

    /*==========================================================================================================================
    | FACTORY METHOD: GET CONFIG
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the <c>topics</c> element from the web.config as the configuation section.
    /// </summary>
    /// <returns>The <c>topics</c> section of the implementing client's configuration.</returns>
    public static TopicsSection GetConfig() {
      return ConfigurationManager.GetSection("topics") as TopicsSection;
    }

    /*==========================================================================================================================
    | ATTRIBUTE: ROOT TOPIC NAMESPACE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets or sets the root (parent) Topic expected to be defined for Topics data.
    /// </summary>
    [ConfigurationProperty("rootTopicNamespace", DefaultValue = "Root")]
    public string RootTopicNamespace {
      get {
        return (string)this["rootTopicNamespace"];
      }
      set {
        this["rootTopicNamespace"] = value;
      }
    }

    /*==========================================================================================================================
    | ATTRIBUTE: TOPIC DELIMITER
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets or sets the delimiter character expected to be used for separating Topics (Topic keys).
    /// </summary>
    [ConfigurationProperty("topicDelimiter", DefaultValue = "/")]
    public string TopicDelimiter {
      get {
        return (string)this["topicDelimiter"];
      }
      set {
        this["topicDelimiter"] = value;
      }
    }

    /*==========================================================================================================================
    | ATTRIBUTE: DEFAULT DATA PROVIDER
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets or sets the default Topics data provider that should be used.
    /// </summary>
    [ConfigurationProperty("defaultDataProvider", DefaultValue = "SqlTopicDataProvider")]
    public string DefaultDataProvider {
      get {
        return (string)this["defaultDataProvider"];
      }
      set {
        this["defaultDataProvider"] = value;
      }
    }

    /*==========================================================================================================================
    | ATTRIBUTE: DEFAULT MAPPING PROVIDER
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets or sets the default Topics mapping provider that should be used.
    /// </summary>
    [ConfigurationProperty("defaultMappingProvider", DefaultValue = "NullTopicMappingProvider")]
    public string DefaultMappingProvider {
      get {
        return (string)this["defaultMappingProvider"];
      }
      set {
        this["defaultMappingProvider"] = value;
      }
    }

    /*==========================================================================================================================
    | ELEMENT: VERSIONING
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the value of the <versioning /> element from the configuration.
    /// </summary>
    [ConfigurationProperty("versioning")]
    public VersioningElement Versioning {
      get {
        return this["versioning"] as VersioningElement;
      }
    }

    /*==========================================================================================================================
    | ELEMENT: EDITOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the value of the <editor /> element from the configuration.
    /// </summary>
    [ConfigurationProperty("editor")]
    public EditorElement Editor {
      get {
        return this["editor"] as EditorElement;
      }
    }

    /*==========================================================================================================================
    | ELEMENT: VIEWS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the value of the <view /> element from the configuration.
    /// </summary>
    [ConfigurationProperty("views")]
    public ViewsElement Views {
      get {
        return this["views"] as ViewsElement;
      }
    }

    /*==========================================================================================================================
    | COLLECTION: PAGE TYPES
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the value of the <pageTypes /> element from the configuration.
    /// </summary>
    [ConfigurationProperty("pageTypes")]
    public PageTypeElementCollection PageTypes {
      get {
        return this["pageTypes"] as PageTypeElementCollection;
      }
    }

    /*==========================================================================================================================
    | COLLECTION: PROVIDERS
    [ConfigurationProperty("providers")]
      public ProviderSettingsCollection Providers {
        get {
          return (ProviderSettingsCollection)base["providers"];
      }
    }
    \-------------------------------------------------------------------------------------------------------------------------*/

    /*==========================================================================================================================
    | COLLECTION: DATA PROVIDERS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the value of the <dataProviders /> element from the configuration.
    /// </summary>
    [ConfigurationProperty("dataProviders")]
    public ProviderSettingsCollection DataProviders {
      get {
        return (ProviderSettingsCollection)base["dataProviders"];
      }
    }

    /*==========================================================================================================================
    | COLLECTION: MAPPING PROVIDERS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Gets the  <mappingProviders /> element from the configuration.
    /// </summary>
    [ConfigurationProperty("mappingProviders")]
    public ProviderSettingsCollection MappingProviders {
      get {
        return (ProviderSettingsCollection)base["mappingProviders"];
      }
    }

  } // Class

} // Namespace

