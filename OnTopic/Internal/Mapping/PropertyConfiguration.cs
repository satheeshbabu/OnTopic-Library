﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using OnTopic.Collections;
using OnTopic.Internal.Diagnostics;
using OnTopic.Mapping;
using OnTopic.Mapping.Annotations;

namespace OnTopic.Internal.Mapping {

  /*============================================================================================================================
  | CLASS: PROPERTY ATTRIBUTES
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Evaluates a <see cref="PropertyInfo"/> instance for known <see cref="Attribute"/>, and exposes them through a set of
  ///   property values.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     The <see cref="PropertyConfiguration"/> class is utilized by implementations of <see cref="ITopicMappingService"/> to
  ///     facilitate the mapping of source <see cref="Topic"/> instances to Data Transfer Objects (DTOs), such as View Models.
  ///     The attribute values provide hints to the mapping service that help manage how the mapping occurs.
  ///   </para>
  ///   <para>
  ///     For example, by default a property on a DTO class will be mapped to a property or attribute of the same name on the
  ///     source <see cref="Topic"/>. If the <see cref="AttributeKeyAttribute"/> is attached to a property on the DTO, however,
  ///     then the <see cref="ITopicMappingService"/> will instead use the value defined by that attribute, thus allowing a
  ///     property on the DTO to be aliased to a different property or attribute name on the source <see cref="Topic"/>.
  ///   </para>
  /// </remarks>
  public class PropertyConfiguration {

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given a <see cref="PropertyInfo"/> instance, exposes a set of properties associated with known <see cref="Attribute"/>
    ///   instances.
    /// </summary>
    /// <param name="property">The <see cref="PropertyInfo"/> instance to check for <see cref="Attribute"/> values.</param>
    /// <param name="attributePrefix">The prefix to apply to the attributes.</param>
    public PropertyConfiguration(PropertyInfo property, string? attributePrefix = "") {

      /*------------------------------------------------------------------------------------------------------------------------
      | Validate parameters
      \-----------------------------------------------------------------------------------------------------------------------*/
      Contract.Requires(property, nameof(property));

      /*------------------------------------------------------------------------------------------------------------------------
      | Set backing property
      \-----------------------------------------------------------------------------------------------------------------------*/
      Property = property;

      /*------------------------------------------------------------------------------------------------------------------------
      | Set default values
      \-----------------------------------------------------------------------------------------------------------------------*/
      AttributeKey              = attributePrefix + property.Name;
      AttributePrefix           = attributePrefix;
      DefaultValue              = null;
      InheritValue              = false;
      RelationshipKey           = AttributeKey;
      RelationshipType          = RelationshipType.Any;
      CrawlRelationships        = Relationships.None;
      MetadataKey               = null;
      DisableMapping            = false;
      AttributeFilters          = new();
      FlattenChildren           = false;

      /*------------------------------------------------------------------------------------------------------------------------
      | Attributes: Retrieve basic attributes
      \-----------------------------------------------------------------------------------------------------------------------*/
      GetAttributeValue<DefaultValueAttribute>(property,        a => DefaultValue = a.Value);
      GetAttributeValue<InheritAttribute>(property,             a => InheritValue = true);
      GetAttributeValue<AttributeKeyAttribute>(property,        a => AttributeKey = attributePrefix + a.Value);
      GetAttributeValue<MapToParentAttribute>(property,         a => MapToParent = true);
      GetAttributeValue<MapToParentAttribute>(property,         a => AttributePrefix += (a.AttributePrefix?? property.Name));
      GetAttributeValue<FollowAttribute>(property,              a => CrawlRelationships = a.Relationships);
      GetAttributeValue<FlattenAttribute>(property,             a => FlattenChildren = true);
      GetAttributeValue<MetadataAttribute>(property,            a => MetadataKey = a.Key);
      GetAttributeValue<DisableMappingAttribute>(property,      a => DisableMapping = true);

      /*------------------------------------------------------------------------------------------------------------------------
      | Attributes: Determine relationship key and type
      \-----------------------------------------------------------------------------------------------------------------------*/
      GetAttributeValue<RelationshipAttribute>(
        property,
        a => {
          RelationshipKey = a.Key ?? RelationshipKey;
          RelationshipType = a.Type;
        }
      );

      if (
        RelationshipType is RelationshipType.Any &&
        RelationshipKey.Equals("Children", StringComparison.InvariantCultureIgnoreCase)
      ) {
        RelationshipType = RelationshipType.Children;
      }

      /*------------------------------------------------------------------------------------------------------------------------
      | Attributes: Set attribute filters
      \-----------------------------------------------------------------------------------------------------------------------*/
      var filterByAttribute = property.GetCustomAttributes<FilterByAttributeAttribute>(true);
      if (filterByAttribute is not null && filterByAttribute.Any()) {
        foreach (var filter in filterByAttribute) {
          AttributeFilters.Add(filter.Key, filter.Value);
        }
      }

    }

    /*==========================================================================================================================
    | PROPERTY: PROPERTY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The <see cref="PropertyInfo"/> that the current <see cref="PropertyConfiguration"/> is associated with.
    /// </summary>
    public PropertyInfo Property { get; }

    /*==========================================================================================================================
    | PROPERTY: ATTRIBUTE KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The name of the corresponding attribute in the <see cref="Topic.Attributes"/> instance.  Defaults to the property name
    ///   on DTO.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By default, a property on a DTO class will be mapped to a property or attribute of the same name on the source <see
    ///     cref="Topic"/>. If the <see cref="AttributeKeyAttribute"/> is attached to a property on the DTO, however, then the
    ///     <see cref="ITopicMappingService"/> will instead use the value defined by that attribute, thus allowing a property on
    ///     the DTO to be aliased to a different property or attribute name on the source <see cref="Topic"/>.
    ///   </para>
    ///   <para>
    ///     The <see cref="AttributeKey"/> property corresponds to the <see cref="AttributeKeyAttribute.Value"/> property. It
    ///     can be assigned by decorating a DTO property with e.g. <c>[AttributeKey("AlternateAttributeKey")]</c>.
    ///   </para>
    /// </remarks>
    public string AttributeKey { get; set; }

    /*==========================================================================================================================
    | PROPERTY: MAP TO PARENT?
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Determines whether or not a complex object's properties should be mapped to the topic's attributes.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By default, properties representing complex classes won't be mapped unless they correspond to a narrow set of types
    ///     (such as <see cref="IList{T}"/>). The <see cref="MapToParentAttribute"/> allows this behavior to be overwritten.
    ///     When this occurs, the properties of the referenced object are treated as attributes on the topic.
    ///   </para>
    ///   <para>
    ///     The <see cref="MapToParent"/> property corresponds to the <see cref="MapToParentAttribute"/> attribute. It can be
    ///     assigned by decorating a DTO property with e.g. <c>[MapToParent]</c>. Optionally, an attribute prefix can be
    ///     provided; see <see cref="AttributePrefix"/> for details.
    ///   </para>
    /// </remarks>
    public bool MapToParent { get; set; }

    /*==========================================================================================================================
    | PROPERTY: ATTRIBUTE PREFIX
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The prefix to apply to the <see cref="AttributeKey"/>; used when mapping complex objects.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By default, properties representing complex classes won't be mapped unless they correspond to a narrow set of types
    ///     (such as <see cref="IList{T}"/>). The <see cref="MapToParentAttribute"/> allows this behavior to be overwritten.
    ///     When this occurs, the properties of the referenced object are treated as attributes on the topic. To distinguish
    ///     them from properties on the parent topic, they are (by default) prefixed with the name of the property that the
    ///     complex object is assigned to. Optionally, however, this can be overridden, or even set to <see
    ///     cref="String.Empty"/>. In the latter case, the properties on the child object will be treated as synonymous with the
    ///     properties on the parent object. And, in fact, the same property could even be applied to <i>both</i> the child
    ///     object <i>and</i> the parent object—though this probably doesn't make much sense from a modeling perspective.
    ///   </para>
    ///   <para>
    ///     Be aware that the <see cref="AttributePrefix"/> should <i>only</i> apply to actual attributes on the mapped <see
    ///     cref="Topic"/> entity; it is <i>not</i> intended to be applied to e.g. collections or relationships.
    ///   </para>
    ///   <para>
    ///     The <see cref="AttributePrefix"/> property corresponds to the <see cref="MapToParentAttribute.AttributePrefix"/>
    ///     property. It can be assigned by decorating a DTO property with e.g. <c>[MapToParent(AttributePrefix
    ///     = "AlternateAttributeKey")]</c>.
    ///   </para>
    ///   <para>
    ///     The <see cref="AttributePrefix"/> can be compounded. That is, if a complex object is added to another complex object
    ///     using <see cref="MapToParentAttribute"/>, then the <see cref="AttributePrefix"/> will reflect the combination of
    ///     those two prefixes. This allows potentially very deep object models.
    ///   </para>
    /// </remarks>
    public string? AttributePrefix { get; set; }

    /*==========================================================================================================================
    | PROPERTY: DEFAULT VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The default value to use if no corresponding value can be determined from the source <see cref="Topic"/>.
    /// </summary>
    /// <remarks>
    ///   The <see cref="DefaultValue"/> property corresponds to the <see cref="DefaultValueAttribute.Value"/> property. It can
    ///   be assigned by decorating a DTO property with e.g. <c>[DefaultValue("DefaultValue")]</c>.
    /// </remarks>
    public object? DefaultValue { get; set; }

    /*==========================================================================================================================
    | PROPERTY: INHERIT VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Determines whether the value should be inherited from <see cref="Topic.Parent"/> if the value cannot be identified
    ///   on the source <see cref="Topic"/>.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     The <see cref="InheritValue"/> configuration is only applicable if the value is pulled from the <see
    ///     cref="Topic.Attributes"/> collection. This is the equivalent to calling the <see
    ///     cref="AttributeValueCollection.GetValue(String, Boolean)"/> method with an <c>InheritFromParent</c> parameter set to
    ///     <c>True</c>.
    ///   </para>
    ///   <para>
    ///     The <see cref="InheritValue"/> property corresponds to the <see cref="InheritAttribute"/> being set on a given
    ///     property. It can be assigned by decorating a DTO property with e.g. <c>[Inherit]</c>.
    ///   </para>
    /// </remarks>
    public bool InheritValue { get; set; }

    /*==========================================================================================================================
    | PROPERTY: RELATIONSHIP KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   The name of the relationship that a collection should map to. Defaults to the property name on DTO.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By default, a collection property on a DTO class will be mapped to a corresponding relationship of the same name.
    ///     So, for instance, if the property on the DTO class is called <c>Cousins</c> then the <see
    ///     cref="ITopicMappingService"/> will search <see cref="Topic.Relationships"/>, <see
    ///     cref="Topic.IncomingRelationships"/>, and, finally, <see cref="Topic.Children"/> for an object named <c>Cousins</c>.
    ///     If the <see cref="RelationshipKey"/> is set, however, then that value is used instead, thus allowing the property on
    ///     the DTO to be aliased to a different collection name on the source <see cref="Topic"/>.
    ///   </para>
    ///   <para>
    ///     The <see cref="RelationshipKey"/> property corresponds to the <see cref="RelationshipAttribute.Key"/> property. It
    ///     can be assigned by decorating a DTO property with e.g. <c>[Relationship("AlternateRelationshipKey")]</c>.
    ///   </para>
    /// </remarks>
    public string RelationshipKey { get; set; }

    /*==========================================================================================================================
    | PROPERTY: RELATIONSHIP TYPE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Determines the type of relationship that a collection property corresponds to.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By default, a collection property on a DTO class will attempt to find a match from, in order, <see
    ///     cref="Topic.Relationships"/>, <see cref="Topic.IncomingRelationships"/>, and, finally, <see cref="Topic.Children"/>.
    ///     If the <see cref="RelationshipType"/> is set, however, then the <see cref="ITopicMappingService"/> will <i>only</i>
    ///     map the collection to a relationship of that type. This can be valuable when the <see cref="RelationshipKey"/> might
    ///     be ambiguous between multiple collections.
    ///   </para>
    ///   <para>
    ///     The <see cref="RelationshipType"/> property corresponds to the <see cref="RelationshipAttribute.Type"/> property. It
    ///     can be assigned by decorating a DTO property with e.g. <c>[Relationship("AlternateRelationshipKey",
    ///     RelationshipType.Children)]</c>.
    ///   </para>
    /// </remarks>
    public RelationshipType RelationshipType { get; set; }

    /*==========================================================================================================================
    | PROPERTY: CRAWL RELATIONSHIPS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Determines which relationships, if any, the <see cref="InterfaceMapping"/> service should crawl for the current
    ///   property.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By default, the all relationships will be mapped on the target DTO, unless the caller specifies otherwise. On any
    ///     related DTOs, however, only <see cref="RelationshipType.NestedTopics"/> will be mapped. So, if a mapped DTO has a
    ///     collection for children, relationships, or even a parent property then any relationships on <i>those</i> DTOs will
    ///     not be mapped. This behavior can be changed by specifying the <see cref="CrawlRelationships"/> flag, which allows
    ///     one or multiple relationships to be specified for a given property.
    ///   </para>
    ///   <para>
    ///     The <see cref="CrawlRelationships"/> property corresponds to the <see cref="FollowAttribute.Relationships"/>
    ///     property. It can be assigned by decorating a DTO property with e.g. <c>[Follow(Relationships.Children)]</c>.
    ///   </para>
    /// </remarks>
    public Relationships CrawlRelationships { get; set; }

    /*==========================================================================================================================
    | PROPERTY: METADATA KEY
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Instructs the <see cref="ITopicMappingService"/> to pull in a collection of mapped topics from a corresponding
    ///   collection in the <c>Root:Configuration:Metadata</c> namespace of the Topic graph.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Sometimes, a <see cref="Topic.Attributes"/> value will be associated with lookup <see cref="Topic"/>s found in the
    ///     <c>Root:Configuration:Metadata</c> namespace of the Topic graph. In these cases, it can be useful to include the
    ///     full set of metadata on the mapped DTO so the view has access to it. For instance, a DTO may include a <c>States</c>
    ///     property which includes a full list of all states potentially associated with a <see cref="Topic"/>, which might be
    ///     used in the user interface to provide filtering of e.g. child <see cref="Topic"/>s.
    ///   </para>
    ///   <para>
    ///     The <see cref="MetadataKey"/> property corresponds to the <see cref="MetadataAttribute.Key"/> property. It can be
    ///     assigned by decorating a DTO property with e.g. <c>[Metadata("States")]</c>.
    ///   </para>
    /// </remarks>
    public string? MetadataKey { get; set; }

    /*==========================================================================================================================
    | PROPERTY: FLATTEN CHILDREN
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Determines whether all descendants in a collection should be included in the collection.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By default, only <see cref="Topic"/> stored directly in a source collection will be included in the target
    ///     collection on a DTO class. If the <see cref="FlattenChildren"/> property is set, however, then all descendants of
    ///     those <see cref="Topic"/>s are <i>also</i> included. So, for instance, if a <c>Children</c> property is decorated
    ///     with the <see cref="FlattenAttribute"/>, then not only will the <see cref="Topic.Children"/> be included, but any
    ///     grandchildren, great-grandchildren, etc. will be included.
    ///   </para>
    ///   <para>
    ///     When <see cref="FlattenChildren"/> is specified, <see cref="ITopicMappingService"/> implementations are expected to
    ///     apply all other constraints. For instance, if the target collection is strongly typed, then only DTOs that exhibit
    ///     polymorphic compatibility with that type will be included (i.e., DTOs of the same or a derived type as the target
    ///     collection). Similarly, any <see cref="AttributeFilters"/> will be applied to each of the descendants. Finally, if
    ///     the target collection has a unique constraint (e.g., due to implementing <see cref="KeyedCollection{TKey, TItem}"/>)
    ///     then any items that would constitute a duplicate will be filtered out <i>without raising an exception</i>. This
    ///     can thus allow the <see cref="FlattenChildren"/> method to be used to represent unique search results within a
    ///     <see cref="Topic"/> graph.
    ///    </para>
    ///   <para>
    ///     The <see cref="FlattenChildren"/> property corresponds to the <see cref="FlattenAttribute"/> being set on a given
    ///     property. It can be assigned by decorating a DTO property with e.g. <c>[Flatten]</c>.
    ///   </para>
    /// </remarks>
    public bool FlattenChildren { get; set; }

    /*==========================================================================================================================
    | PROPERTY: DISABLE MAPPING
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Determines whether the property is intended to be mapped to the corresponding <see cref="Topic"/>.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     The <see cref="DisableMapping"/> property corresponds to the <see cref="DisableMappingAttribute"/> being set on a
    ///     given property. It can be assigned by decorating a DTO property with e.g. <c>[DisableMapping]</c>.
    ///   </para>
    /// </remarks>
    public bool DisableMapping { get; set; }

    /*==========================================================================================================================
    | PROPERTY: ATTRIBUTE FILTERS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Provides a list of <c>Key</c>/<c>Value</c> pairs corresponding to <see cref="Topic.Attributes"/> which can optionally
    ///   be used to filter a collection.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By default, all <see cref="Topic"/>s in a source collection (e.g., <see cref="Topic.Children"/>) will be included in
    ///     a corresponding collection on the DTO (assuming the mapped DTO is compatible with the collection type). If any
    ///     <see cref="AttributeFilters"/> are set, however, then each <see cref="Topic"/> will be evaluated to confirm that it
    ///     satisfies the conditions of those filters.
    ///   </para>
    ///   <para>
    ///     The <see cref="AttributeFilters"/> property corresponds to the <see cref="FilterByAttributeAttribute.Key"/> and
    ///     <see cref="FilterByAttributeAttribute.Value"/> properties. It can be assigned by decorating a DTO property with e.g.
    ///     <c>[FilterByAttribute("Company", "Ignia")]</c>. Multiple <see cref="FilterByAttributeAttribute"/>s can be assigned
    ///     to a single property, thus allowing each item in a collection to be filtered by multiple values.
    ///   </para>
    /// </remarks>
    public Dictionary<string, string> AttributeFilters { get; }

    /*==========================================================================================================================
    | METHOD: SATISFIES ATTRIBUTE FILTERS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given a <see cref="Topic"/>, determines whether or not the <see cref="Topic.Attributes"/> satisfy all <see
    ///   cref="AttributeFilters"/> defined on the property.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool SatisfiesAttributeFilters(Topic source) =>
      AttributeFilters.All(f =>
        source?.Attributes?.GetValue(f.Key, "")?.Equals(f.Value, StringComparison.InvariantCultureIgnoreCase)?? false
      );

    /*==========================================================================================================================
    | METHOD: VALIDATE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Given a target DTO, will automatically identify any attributes that derive from <see cref="ValidationAttribute"/> and
    ///   ensure that their conditions are satisfied.
    /// </summary>
    /// <param name="target">The target DTO to validate the current property on.</param>
    public void Validate(object target) {
      foreach (ValidationAttribute validator in Property.GetCustomAttributes(typeof(ValidationAttribute))) {
        validator.Validate(Property.GetValue(target), Property.Name);
      }
    }

    /*==========================================================================================================================
    | PRIVATE: GET ATTRIBUTE VALUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Helper function evaluates an attribute and then, if it exists, executes an <see cref="Action{T1}"/> to process the
    ///   results.
    /// </summary>
    /// <typeparam name="T">An <see cref="Attribute"/> type to evaluate.</typeparam>
    /// <param name="property">The <see cref="PropertyInfo"/> instance to pull the attribute from.</param>
    /// <param name="action">The <see cref="Action{T}"/> to execute on the attribute.</param>
    private static void GetAttributeValue<T>(PropertyInfo property, Action<T> action) where T : Attribute {
      var attribute = (T)property.GetCustomAttribute(typeof(T), true);
      if (attribute is not null) {
        action(attribute);
      }
    }

  } //Class
} //Namespace