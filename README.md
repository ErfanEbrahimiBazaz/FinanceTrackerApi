# FinanceTracker API – Lessons Learned & Architecture Notes

This README is **not** a project description. It is a **knowledge log** capturing important architectural decisions, pitfalls, best practices, and lessons learned while building this .NET / EF Core / AutoMapper–based API.

The goal is simple:
> When I open this repository weeks or months later, I should immediately remember **why things were done this way** and **what mistakes to avoid repeating**.

---

## 1. Dependency Injection (DI) – EF Core & Lifetimes

### 1.1 DbContext lifetime

- `DbContext` **must be scoped**
- Correct registration:

```csharp
services.AddDbContext<AppDbContext>(options => ...);
```

- ❌ **Never** register `DbContext` as `Singleton`
- ❌ Avoid manually creating `DbContext` with `new`

**Why?**
- DbContext is **not thread-safe**
- Singleton DbContext leads to:
  - Memory leaks
  - Cross-request data corruption
  - Concurrency exceptions

---

### 1.2 InMemoryDatabase – testing & pitfalls

```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));
```

**Important lessons:**
- InMemory provider:
  - Does **NOT** behave like a relational database
  - Does **NOT** enforce constraints, joins, or SQL semantics
- Good for:
  - Unit tests
  - Very fast prototyping
- Bad for:
  - Verifying SQL behavior
  - Performance testing

📌 **Rule:**
> Passing tests on InMemory does not guarantee correctness on SQL Server

---

## 2. Validation – Where It Belongs

### 2.1 ModelState & Model Binding

- `ModelState` exists **only** in MVC layer
- Automatically populated during **model binding** (HTTP → DTO)

```csharp
[ApiController]
public class AccountsController : ControllerBase
{
    public IActionResult Create(AccountsForCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
    }
}
```

- ❌ `ModelState` **does not belong** in:
  - Repository
  - DbContext
  - Domain entities

---

### 2.2 Data Annotations vs Fluent API vs Manual Validation

#### Data Annotations

```csharp
[Required]
[StringLength(20)]
public string AccountNumber { get; set; }
```

- Pros:
  - Simple
  - Integrated with ModelState
- Cons:
  - UI-focused
  - Weak for complex business rules

#### Fluent API

```csharp
modelBuilder.Entity<Account>()
    .Property(a => a.AccountNumber)
    .IsRequired();
```

- Pros:
  - Database-level guarantees
  - Stronger than annotations

#### Manual Validation (reflection-based)

```csharp
Validator.ValidateObject(entity, context, true);
```

- ⚠️ Slow
- ⚠️ Reflection-based
- ❌ Not recommended in hot paths

📌 **Best practice:**
> Use annotations for API input validation, Fluent API for DB constraints, and domain rules for invariants.

---

## 3. Domain Entities & Invariants (DDD Concepts)

### 3.1 What is an invariant?

An **invariant** is a rule that must **always be true** for an entity.

Examples:
- Balance must never be negative
- Account number must always exist

```csharp
public Account(string accountNumber, decimal balance)
{
    if (string.IsNullOrWhiteSpace(accountNumber))
        throw new ArgumentException();
    if (balance < 0)
        throw new ArgumentOutOfRangeException();
}
```

---

### 3.2 Rich entities vs POCOs

**Two styles:**

| Style | Description |
|-----|------------|
| Anemic | POCOs with no behavior |
| Rich | Entities enforce invariants |

**Lesson learned:**
- DTOs → **always POCOs**
- Entities → can be **rich**, but avoid overengineering

📌 Stripe / Netflix-style rule:
> "Never allow mapping INTO aggregates"

Meaning:
- ❌ DTO → Entity AutoMapper mappings
- ✅ Entity → DTO only

---

## 4. AutoMapper – Power & Pitfalls

### 4.1 ReverseMap() – why it’s dangerous

```csharp
CreateMap<Account, AccountDto>().ReverseMap();
```

**Why this is risky:**
- Allows external input to overwrite domain state
- Can bypass invariants
- Can accidentally update PKs or navigation properties

📌 **Rule:**
> Only use `ReverseMap()` if you fully control both sides

---

### 4.2 AutoMapper version compatibility

**Important pitfall encountered:**

- `AutoMapper`
- `AutoMapper.Extensions.Microsoft.DependencyInjection`

❗ Versions **must match**

Example:
- AutoMapper 13.x + Extensions 12.x → runtime failures

📌 Always align versions explicitly.

---

## 5. ProjectTo() – Projection & Performance

### 5.1 What ProjectTo() does

```csharp
context.Accounts
    .ProjectTo<AccountDto>(mapper.ConfigurationProvider)
```

- Translates mapping into SQL
- Avoids materializing entities
- Fetches only required columns

---

### 5.2 When ProjectTo() is a win

- Large tables
- Read-heavy endpoints
- DTOs with few fields

---

### 5.3 When ProjectTo() is NOT a win

Observed behavior:
- More complex SQL
- Extra joins
- Slightly worse performance for:
  - Small datasets
  - Aggregates with collections

📌 **Lesson:**
> ProjectTo is not magic – always inspect generated SQL

---

## 6. Controllers, Repositories & Responsibilities

### 6.1 Controller responsibilities

- HTTP concerns
- ModelState validation
- Status codes
- Routing

### 6.2 Repository responsibilities

- Data access only
- No ModelState
- No HTTP logic

📌 Repositories should not:
- Return IActionResult
- Know about DTO validation

---

## 7. Error Handling & Production Safety

### 7.1 Runtime exceptions

Example:
```text
KeyNotFoundException: Account with ID 2 not found
```

**Important lesson:**
- This does **NOT** crash the server
- Only the request fails
- Other endpoints continue to work

---

### 7.2 Proper production setup

- Global exception handling middleware
- No stack traces in production
- Structured error responses

---

## 8. SQL Server Connection Strings

### 8.1 LocalDB vs SQL Server

```text
(localdb)\\mssqllocaldb
```

- LocalDB ≠ SQL Server Express
- It does **NOT** map to machine name
- It is a developer-only lightweight instance

For real SQL Server:
```text
Server=FALCONZANPAKUTO;Database=FinanceTrackerDb;Trusted_Connection=True;
```

---

### 8.2 Encrypt vs TrustServerCertificate

| Setting | Meaning |
|------|--------|
| Encrypt=true | Encrypted connection |
| TrustServerCertificate=true | Skip certificate validation |

📌 Production:
- Encrypt=true
- TrustServerCertificate=false

---

## 9. Git – Cherry-pick, Rebase & History Hygiene

### 9.1 Cherry-pick

- Copies commits between branches
- Generates **new SHAs**
- Can cause duplicates if misused

---

### 9.2 Interactive rebase (important lesson)

Used to:
- Remove unwanted commits
- Clean history before pushing

```bash
git rebase -i <base>
```

📌 Safe only if commits are **not yet shared**.

---

### 9.3 Mental model

- Rebase = rewrite history
- Cherry-pick = copy commits
- Merge = connect histories

---

## 10. Terminology Refresher

| Term | Meaning |
|----|--------|
| TL;DR | Too Long; Didn’t Read |
| Model Binding | HTTP → object mapping |
| Projection | Query → DTO without entity |
| Invariant | Rule that must always hold |
| Aggregate | Consistency boundary in DDD |

---

## Final Guiding Principles

1. **Validate early (controller)**
2. **Protect invariants (domain)**
3. **Project for reads, entities for writes**
4. **Never blindly trust AutoMapper**
5. **Inspect generated SQL**
6. **Keep Git history intentional**

---

> This repository is not just code.
> It is a record of architectural thinking and hard-earned lessons.

