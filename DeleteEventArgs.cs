/*=========================================================================================================================
| DELETE EVENT ARGS
|
| Author:    Casey Margell, Ignia LLC (casey.margell@ignia.com)
| Client     Microsoft
| Project    AdCenter
|
| Purpose :  The DeleteEventArgs object defines an event argument type specific to deletion events
|
>=========================================================================================================================
| Revisions  Date        Author          Comments
| - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
|            04.01.09    Casey Margell   Moved this class to it's own file
\------------------------------------------------------------------------------------------------------------------------*/
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

/*=========================================================================================================================
| NAMESPACE: IGNIA.TOPICS
>==========================================================================================================================
| Contains objects associated with the Ignia Topics
\------------------------------------------------------------------------------------------------------------------------*/
  namespace Ignia.Topics {

  /*------------------------------------------------------------------------------------------------------------------------
  | CLASS: TAXONOMY DELETE EVENT ARGS
  >-------------------------------------------------------------------------------------------------------------------------
  | Event arguments for delete events
  \------------------------------------------------------------------------------------------------------------------------*/
    public class DeleteEventArgs : EventArgs {
      private Topic _topic = null;

    /*------------------------------------------------------------------------------------------------------------------------
    | CONSTRUCTOR: TAXONOMY DELETE EVENT ARGS
    >-------------------------------------------------------------------------------------------------------------------------
    | Constructor for a delete event args object.
    \------------------------------------------------------------------------------------------------------------------------*/
      public DeleteEventArgs(Topic topic) : base() {
        _topic = topic;
        }

    /*------------------------------------------------------------------------------------------------------------------------
    | PROPERTY: TOPIC
    >-------------------------------------------------------------------------------------------------------------------------
    | Getter that returns the Topic object associated with the event
    \------------------------------------------------------------------------------------------------------------------------*/
      public Topic Topic {
        get {
          return _topic;
          }
        set {
          _topic = value;
          }
        }
      }
    }