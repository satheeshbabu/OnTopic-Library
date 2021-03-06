﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnTopic.Internal.Diagnostics;

namespace OnTopic.Tests {

  /*============================================================================================================================
  | CLASS: CONTRACT TEST
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides unit tests for the <see cref="Contract"/> class.
  /// </summary>
  [TestClass]
  public class ContractTest {

    /*==========================================================================================================================
    | TEST: REQUIRES: CONDITION IS TRUE: THROW NO EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tests a true condition using the <see cref="Contract"/> class, and validates that it correctly does nothing.
    /// </summary>
    [TestMethod]
    public void Requires_ConditionIsTrue_ThrowNoException()
      => Contract.Requires(true, "The argument cannot be null");

    /*==========================================================================================================================
    | TEST: REQUIRES: CONDITION IS FALSE: THROW ARGUMENT NULL EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tests a false condition using the <see cref="Contract"/> class, and validates that it correctly returns an <see
    ///   cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Requires_ConditionIsFalse_ThrowArgumentNullException()
      => Contract.Requires(false, "The argument cannot be null");

    /*==========================================================================================================================
    | TEST: REQUIRES: OBJECT EXISTS: THROW NO EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tests a non-null argument using the <see cref="Contract"/> class, and validates that it correctly does nothing.
    /// </summary>
    [TestMethod]
    public void Requires_ObjectExists_ThrowNoException() =>
      Contract.Requires(new object(), "The argument cannot be null");

    /*==========================================================================================================================
    | TEST: REQUIRES: OBJECT IS NULL: THROW ARGUMENT NULL EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tests a null argument using the <see cref="Contract"/> class, and validates that it correctly returns an <see
    ///   cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Requires_ObjectIsNull_ThrowArgumentNullException() =>
      Contract.Requires(null, "The argument cannot be null");

    /*==========================================================================================================================
    | TEST: REQUIRES: MESSAGE EXISTS: THROW EXCEPTION WITH MESSAGE
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tests a null argument using the <see cref="Contract"/> class, and validates that it correctly returns an <see
    ///   cref="ArgumentNullException"/> with the expected <see cref="ArgumentException.Message"/>.
    /// </summary>
    [TestMethod]
    public void Requires_MessageExists_ThrowExceptionWithMessage() {

      var argument = (object?)null;
      var errorMessage = "The argument cannot be null";

      try {
        Contract.Requires<ArgumentException>(argument is not null, errorMessage);
      }
      catch (ArgumentException ex) {
        Assert.AreEqual<String>(errorMessage, ex.Message);
      }

    }

    /*==========================================================================================================================
    | TEST: ASSUME: CONDITION IS TRUE: THROW NO EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tests a true condition using the <see cref="Contract"/> class, and validates that it correctly does nothing.
    /// </summary>
    [TestMethod]
    public void Assume_ConditionIsTrue_ThrowNoException()
      => Contract.Assume(true, "The argument cannot be null");

    /*==========================================================================================================================
    | TEST: ASSUME: CONDITION IS FALSE: THROW ARGUMENT NULL EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tests a false condition using the <see cref="Contract"/> class, and validates that it correctly returns an <see
    ///   cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void Assume_ConditionIsFalse_ThrowArgumentNullException()
      => Contract.Assume(false, "The argument cannot be null");

    /*==========================================================================================================================
    | TEST: ASSUME: OBJECT IS NULL: THROW INVALID OPERATION EXCEPTION
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Tests a null assignment using the <see cref="Contract"/> class, and validates that it correctly returns an <see
    ///   cref="InvalidOperationException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Assume_ObjectIsNull_ThrowInvalidOperationException()
      => Contract.Assume(null, "The local runtime state is invalid.");

  } //Class
} //Namespace