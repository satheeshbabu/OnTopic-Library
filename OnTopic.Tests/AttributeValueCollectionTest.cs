﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnTopic.Attributes;
using OnTopic.Collections;

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
    | TEST: GET DOUBLE: CORRECT VALUE: IS RETURNED
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that double values can be set and retrieved as expected.
    /// </summary>
    [TestMethod]
    public void GetDouble_CorrectValue_IsReturned() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetDouble("Number1", 1);

      Assert.AreEqual<double>(1.0, topic.Attributes.GetDouble("Number1", 5.0));

    }

    /*==========================================================================================================================
    | TEST: GET DOUBLE: INCORRECT VALUE: RETURNS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that double values return the default.
    /// </summary>
    [TestMethod]
    public void GetDouble_IncorrectValue_ReturnsDefault() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Number3", "Invalid");

      Assert.AreEqual<double>(5.0, topic.Attributes.GetDouble("Number3", 5.0));

    }

    /*==========================================================================================================================
    | TEST: GET DOUBLE: INCORRECT KEY: RETURNS DEFAULT
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Ensures that double key names return the default.
    /// </summary>
    [TestMethod]
    public void GetDouble_IncorrectKey_ReturnsDefault() {

      var topic = TopicFactory.Create("Test", "Container");

      Assert.AreEqual<double>(5.0, topic.Attributes.GetDouble("InvalidKey", 5.0));

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
    ///   Sets the value of a custom <see cref="AttributeValue"/> to the existing value and ensures it is <i>not</i> marked as
    ///   <see cref="AttributeValue.IsDirty"/>.
    /// </summary>
    [TestMethod]
    public void SetValue_ValueUnchanged_IsNotDirty() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Fah", "Bar", false);
      topic.Attributes.SetValue("Fah", "Bar");

      Assert.IsFalse(topic.Attributes["Fah"].IsDirty);

    }

    /*==========================================================================================================================
    | TEST: IS DIRTY: DIRTY VALUES: RETURNS TRUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Populates the <see cref="AttributeValueCollection"/> with a <see cref="AttributeValue"/> that is marked as <see
    ///   cref="AttributeValue.IsDirty"/>. Confirms that <see cref="AttributeValueCollection.IsDirty(Boolean)"/> returns
    ///   <c>true</c>.
    /// </summary>
    [TestMethod]
    public void IsDirty_DirtyValues_ReturnsTrue() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Foo", "Bar");

      Assert.IsTrue(topic.Attributes.IsDirty());

    }

    /*==========================================================================================================================
    | TEST: IS DIRTY: DELETED VALUES: RETURNS TRUE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Populates the <see cref="AttributeValueCollection"/> with a <see cref="AttributeValue"/> and then deletes it. Confirms
    ///   that <see cref="AttributeValueCollection.IsDirty(Boolean)"/> returns <c>true</c>.
    /// </summary>
    [TestMethod]
    public void IsDirty_DeletedValues_ReturnsTrue() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Foo", "Bar");
      topic.Attributes.Remove("Foo");

      Assert.IsTrue(topic.Attributes.IsDirty());

    }

    /*==========================================================================================================================
    | TEST: IS DIRTY: NO DIRTY VALUES: RETURNS FALSE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Populates the <see cref="AttributeValueCollection"/> with a <see cref="AttributeValue"/> that is <i>not</i> marked as
    ///   <see cref="AttributeValue.IsDirty"/>. Confirms that <see cref="AttributeValueCollection.IsDirty(Boolean)"/> returns
    ///   <c>false</c>/
    /// </summary>
    [TestMethod]
    public void IsDirty_NoDirtyValues_ReturnsFalse() {

      var topic = TopicFactory.Create("Test", "Container", 1);

      topic.Attributes.SetValue("Foo", "Bar", false);

      Assert.IsFalse(topic.Attributes.IsDirty());

    }

    /*==========================================================================================================================
    | TEST: IS DIRTY: EXCLUDE LAST MODIFIED: RETURNS FALSE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Populates the <see cref="AttributeValueCollection"/> with a <see cref="AttributeValue"/> that is <i>not</i> marked as
    ///   <see cref="AttributeValue.IsDirty"/> as well as a <c>LastModified</c> <see cref="AttributeValue"/> that is. Confirms
    ///   that <see cref="AttributeValueCollection.IsDirty(Boolean)"/> returns <c>false</c>.
    /// </summary>
    [TestMethod]
    public void IsDirty_ExcludeLastModified_ReturnsFalse() {

      var topic = TopicFactory.Create("Test", "Container", 1);

      topic.Attributes.SetValue("Foo", "Bar", false);
      topic.Attributes.SetValue("LastModified", DateTime.Now.ToString(CultureInfo.InvariantCulture));
      topic.Attributes.SetValue("LastModifiedBy", "System");

      Assert.IsFalse(topic.Attributes.IsDirty(true));

    }

    /*==========================================================================================================================
    | TEST: IS DIRTY: MARK CLEAN: RETURNS FALSE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Populates the <see cref="AttributeValueCollection"/> with a <see cref="AttributeValue"/> and then deletes it. Confirms
    ///   that <see cref="AttributeValueCollection.IsDirty(Boolean)"/> returns <c>false</c> after calling <see cref="
    ///   AttributeValueCollection.MarkClean(DateTime?)"/>.
    /// </summary>
    [TestMethod]
    public void IsDirty_MarkClean_ReturnsFalse() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Foo", "Bar");
      topic.Attributes.SetValue("Baz", "Foo");

      topic.Attributes.Remove("Foo");

      topic.Attributes.MarkClean();

      Assert.IsFalse(topic.Attributes.IsDirty());

    }

    /*==========================================================================================================================
    | TEST: IS DIRTY: MARK ATTRIBUTE CLEAN: RETURNS FALSE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Populates the <see cref="AttributeValueCollection"/> with a <see cref="AttributeValue"/> and then confirms that <see
    ///   cref="AttributeValueCollection.IsDirty(String)"/> returns <c>false</c> for that attribute after calling <see cref="
    ///   AttributeValueCollection.MarkClean(String, DateTime?)"/>.
    /// </summary>
    [TestMethod]
    public void IsDirty_MarkAttributeClean_ReturnsFalse() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Foo", "Bar");
      topic.Attributes.MarkClean("Foo");

      Assert.IsFalse(topic.Attributes.IsDirty("Foo"));

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
      topic.Attributes.Add(new("Key", "NewKey", false));

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
      topic.Attributes.Add(new("Key", "# ?"));
    }

    /*==========================================================================================================================
    | TEST: SET VALUE: EMPTY ATTRIBUTE VALUE: SKIPS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Adds a new attribute with an empty value, and confirms that it is <i>not</i> added as a new <see
    ///   cref="AttributeValue"/>. Empty values are treated as the same as non-existent attributes. They are stored for the sake
    ///   of tracking <i>deleted</i> attributes, but should not be stored for <i>new</i> attributes.
    /// </summary>
    [TestMethod]
    public void SetValue_EmptyAttributeValue_Skips() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Attribute", "");

      Assert.IsFalse(topic.Attributes.Contains("Attribute"));

    }

    /*==========================================================================================================================
    | TEST: SET VALUE: UPDATE EMPTY ATTRIBUTE VALUE: REPLACES
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Adds a new attribute with an empty value, and confirms that it is <i>is</i> added as a new <see
    ///   cref="AttributeValue"/> assuming the value previously existed. Empty values are treated as the same as non-existent
    ///   attributes, but they should be stored for the sake of tracking <i>deleted</i> attributes.
    /// </summary>
    [TestMethod]
    public void SetValue_EmptyAttributeValue_Replaces() {

      var topic = TopicFactory.Create("Test", "Container");

      topic.Attributes.SetValue("Attribute", "New Value");
      topic.Attributes.SetValue("Attribute", "");

      Assert.IsTrue(topic.Attributes.Contains("Attribute"));

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