# 🧰 Domain Event Dispatcher without any reflections

This repository demonstrates how to implement a **domain event dispatcher** in .NET without relying on reflection at runtime, using **keyed service registration** instead. The goal is to maintain type safety, performance, and separation of concerns when dispatching domain events.

## 🔍 Motivation

Reflection is commonly used to resolve domain event handlers dynamically. However, it introduces performance overhead and complexity. This repo shows how to **eliminate reflection** during event dispatch by registering handlers using `.AddKeyedScoped()` and resolving them via keyed services.

## 🛠️ Key Features

- ✅ Avoids reflection for handler resolution  
- ✅ Clean registration of domain event handlers by interface  
- ✅ Uses EF Core's `SaveChangesInterceptor` to dispatch events automatically  
- ✅ Supports multiple handlers per event  
- ✅ Testable, extensible, and performant

## 📁 Project Structure

- `Abstractions/` - Base interfaces for domain events and handlers  
- `Data/` - `AppDbContext` and EF interceptors  
- `EventHandlers/` - Example handler(s)  
- `Domain/` - Aggregates and events

## 🔄 Event Dispatching Flow

1. **Domain events** are raised inside aggregate roots.
2. During `SaveChangesAsync`, `DispatchDomainEventsInterceptor` extracts and clears these events.
3. Handlers are resolved via `GetKeyedServices` using the pattern `${eventTypeName}Handler`.
4. All handlers are invoked in parallel (sequentially in current demo).