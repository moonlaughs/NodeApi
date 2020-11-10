PRAGMA foreign_keys=off;

BEGIN TRANSACTION;
DROP TABLE nodepairs_old;
drop table nodes_old;
ALTER TABLE nodePairs RENAME to nodepairs_old;

CREATE table nodePairs(
    [parentNodeId] INTEGER NOT NULL,
    [childNodeId] INTEGER NOT NULL,
    
    FOREIGN KEY (parentNodeId) REFERENCES nodes(nodeId),
    FOREIGN KEY (childNodeId) REFERENCES nodes(nodeId),
    CONSTRAINT PK_Pair PRIMARY KEY (parentNodeId,childNodeId)
);

insert into nodePairs select * from nodepairs_old;

COMMIT;
PRAGMA foreign_keys=on;

PRAGMA foreign_keys=off;

BEGIN TRANSACTION;
ALTER TABLE nodes RENAME to nodes_old;

CREATE table nodes(
    [nodeId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    [nodeName] VARCHAR(50) NOT NULL
);

insert into nodes select * from nodes_old;

COMMIT;
PRAGMA foreign_keys=on;