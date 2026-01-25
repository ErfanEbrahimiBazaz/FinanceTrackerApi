 -- EF qeury without IMApper's QueryableExtensions' ProjectTo()
 System.InvalidOperationException: 'Missing map from FinanceTrackerApi.Entities.Transaction to FinanceTrackerApi.Dto.TransactionDto. Create using CreateMap<Transaction, TransactionDto>.'


 Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (36ms) [Parameters=[@__id_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [a0].[Id], [a0].[AccountNumber], [a0].[Balance], [t].[Id], [t].[AccountId], [t].[Amount], [t].[Date], [t].[Description], [t].[Type]
      FROM (
          SELECT TOP(1) [a].[Id], [a].[AccountNumber], [a].[Balance]
          FROM [Accounts] AS [a]
          WHERE [a].[Id] = @__id_0
      ) AS [a0]
      LEFT JOIN [Transactions] AS [t] ON [a0].[Id] = [t].[AccountId]
      ORDER BY [a0].[Id]

-- EF qeury WITH IMApper's QueryableExtensions' ProjectTo()
 Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (44ms) [Parameters=[@__id_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
      SELECT [a1].[Id], [a1].[AccountNumber], [a1].[Balance], [s].[Id], [s].[AccountId], [s].[c], [s].[Id0], [s].[AccountNumber], [s].[Balance], [s].[Amount], [s].[Type], [s].[Description]
      FROM (
          SELECT TOP(1) [a].[Id], [a].[AccountNumber], [a].[Balance]
          FROM [Accounts] AS [a]
          WHERE [a].[Id] = @__id_0
      ) AS [a1]
      LEFT JOIN (
          SELECT [t].[Id], [t].[AccountId], CAST(0 AS bit) AS [c], [a0].[Id] AS [Id0], [a0].[AccountNumber], [a0].[Balance], [t].[Amount], [t].[Type], [t].[Description]
          FROM [Transactions] AS [t]
          INNER JOIN [Accounts] AS [a0] ON [t].[AccountId] = [a0].[Id]
      ) AS [s] ON [a1].[Id] = [s].[AccountId]
      ORDER BY [a1].[Id], [s].[Id]