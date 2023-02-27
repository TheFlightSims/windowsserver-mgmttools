USE [PDKDB];
GO

SELECT 
    ProductDescription, ProductKey
FROM
  ( SELECT ProductDescription, ProductKey, 
           ROW_NUMBER() OVER (PARTITION BY ProductDescription
                                ORDER BY ProductDescription
                             ) AS rn
    FROM [WindowsServers]
    GROUP BY ProductDescription, ProductKey
  ) AS t
WHERE
    rn <= 20
ORDER BY 
    ProductDescription, rn;