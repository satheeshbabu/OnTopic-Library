# SQL Schema
In addition to the objects which are part of the [default `[dbo]` schema](../README.md), the database project also includes a `[utilities]` schema for internal maintenance-oriented procedures.

> *Note:* Not all SQL objects are documented here. Missing objects are primarily intended for infrastructure support and used exclusively by stored procedures or administrators.

## Stored Procedures
The following is a summary of the most relevant stored procedures.

- **[`CompressHierarchy`](Stored%20Procedures/CompressHierarchy.sql)**: Occassionally, gaps can occur in the nested set ranges. These don't hurt anything, but they can make it difficult to troubleshoot issues. This procedure removes any gaps.
- **[`ConsolidateVersions`](Stored%20Procedures/ConsolidateVersions.sql)**: Given two dates, will consolidate all versions within the range to a single version. This is useful for reducing the resolution of old versions.
- **[`DeleteConsecutiveAttributes`](Stored%20Procedures/DeleteConsecutiveAttributes.sql)**: In previous versions of OnTopic, some indexed attributes could end up with duplicate versions even though their values hadn't changed. This procedure identifies and removes these without losing any relevant version information.
- **[`DeleteConsecutiveExtendedAttributes`](Stored%20Procedures/DeleteConsecutiveExtendedAttributes.sql)***: In previous versions of OnTopic, the extended attributes XML was duplicated for _every_ version, which can significantly increase the size of the database. This procedure identified and removes consecutive duplicates without losing any relevant version information.
- **[`DeleteOrphanedLastModifiedAttributes`](Stored%20Procedures/DeleteOrphanedLastModifiedAttributes.sql)**: Whenever a topic is saved via the OnTopic Editor,  a new dateline (`LastModified`) attribute is saved—_even_ if no (other) attributes were modified. This can artificially bloat the history with phantom versions. This procedure removes `LastModified` attributes that don't map to other attribute changes.
- **[`ValidateHierarchy`](Stored%20Procedures/ValidateHierarchy.sql)***: The nested set hierarchy can be sensitive to concurrency issues. To mitigate against that, newer versions of OnTopic use transactions to avoid potential corruption when deleting or moving topics in the hierarchy. This procedure identies potential sources of corruption, but does not resolve them. 

> _Note:_ Procedures marked with an asterisk (\*) can be very resource intensive, and may require upwards of ten minutes to complete, depending on the database size.
