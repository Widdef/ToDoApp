# 📝 ToDoApp

Prosty system zarządzania zadaniami (To-Do List) zbudowany w architekturze **full-stack**:
- Backend: **ASP.NET Core Web API** z **Entity Framework Core** i **SQLite**
- Frontend: **Vue 3** z **Vite**, **Axios** i **Bootstrap 5**

---

## 🚀 Funkcjonalności

- Dodawanie, edycja i usuwanie zadań
- Oznaczanie zadań jako ukończone
- Filtrowanie po:
  - nazwie zadania
  - dacie utworzenia (zakres dat)
  - statusie ukończenia
- Sortowanie po tytule, dacie utworzenia i terminie wykonania
- Obsługa priorytetów (niski / średni / wysoki)
- Interfejs responsywny i estetyczny



Aplikacja domyślnie startuje pod adresem:

http://localhost:5272


Wymagania:

- Node.js 18+
- npm lub pnpm
- .NET 8 SDK
- SQLite

Jak skompilować:<br/>
dotnet restore<br/>
dotnet ef database update<br/>
dotnet run<br/>

Front-end, jest budowany automatycznie podczas buildu dotneta.<br/>
W przypadku chęci uruchomienia front-endu bez API, <br/>
trzeba przejść do katalogu ClientApp i wywołać:
- npm install<br/>
- npm run dev<br/>
Samodzielny frontend startuje pod adresem: http://localhost:5173

wersja release:<br/>
    dotnet publish -c Release -r win-x64 --self-contained true
