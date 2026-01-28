# MoodNest – Journal App

MoodNest is a secure, feature-rich desktop journaling application developed using .NET MAUI Blazor Hybrid. The application enables users to record daily journal entries, track moods, analyze emotional patterns, and maintain consistent journaling habits through streak tracking and analytics.

This project was developed as part of the **CS6004NI – Application Development** module and demonstrates the practical application of modern C#.NET development principles, clean architecture, and software engineering best practices.

---

## Project Overview

MoodNest modernizes traditional journaling by providing a structured digital platform with analytics, privacy controls, and rich writing capabilities. The system supports one journal entry per day, ensuring consistency and accurate timestamping while promoting mindful reflection.

Key goals of the project include:
- Encouraging regular journaling habits
- Supporting emotional awareness through mood tracking
- Providing insightful analytics and trends
- Ensuring data privacy and secure local storage

---

## Features

- Secure PIN-based user authentication
- Multiple user registration with data isolation
- Create, read, update, and delete journal entries (one per day)
- Rich Text and Markdown-based writing
- Mood tracking (one primary, up to two secondary moods)
- Predefined and custom tags and categories
- Calendar-based journal navigation
- Paginated journal list view
- Advanced search and filtering
- Streak tracking (current streak, longest streak, missed days)
- Analytics dashboard (mood distribution, trends, word count, tags)
- Public and private journal sharing
- PDF export of journal entries by date range
- Light and dark theme customization
- Offline-first local data storage

---

## Technology Stack

| Technology | Purpose |
|---------|--------|
| .NET MAUI Blazor Hybrid | Cross-platform desktop UI |
| C# (.NET 9) | Core application logic |
| Entity Framework Core | ORM for database access |
| SQLite | Local database storage |
| LINQ | Querying and data manipulation |
| MVVM Architecture | UI separation and maintainability |
| Dependency Injection | Loose coupling and testability |
| iText 7 | PDF export functionality |
| Git | Version control |

---

## Architecture Overview

The application follows a layered and modular architecture:

- **UI Layer**  
  MAUI Pages and Razor Components for presentation

- **ViewModels**  
  Manages UI state and user interactions

- **Service Layer**  
  Business logic (e.g., `JournalService`, analytics, filtering, export)

- **Data Layer**  
  Entity Framework Core with `AppDbContext`

- **Models & Entities**  
  Domain entities and view models

This structure enforces:
- Separation of Concerns (SoC)
- Single Responsibility Principle (SRP)
- Abstraction through interfaces
- Dependency Injection for loose coupling

---

## Software Engineering Practices

- Clean and readable code with consistent naming and formatting
- Optimized LINQ queries to reduce redundant database calls
- Asynchronous programming for responsive UI
- Centralized error handling using `ServiceResult<T>`
- Reusable service logic across multiple features
- Efficient algorithms for streak tracking and analytics
- Version control with meaningful commits and tags

---

## How to Run the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/RewajNeupane/MoodNest-Journal-App-Application-Development-
