# Rental Equipment Management System

Aplikacja konsolowa w C# do zarządzania wypożyczalnią sprzętu (laptopy, projektory, kamery). Projekt został wykonany na potrzeby ćwiczenia APBD.

## Uruchomienie

Wymagany .NET 8 SDK. W katalogu głównym repozytorium:

```bash
cd RentalApp
dotnet run

## Decyzje projektowe
- **Podział na warstwy**: Models (dane), Services (logika), Data (symulacja bazy), UI (interfejs) – każda warstwa ma jedną odpowiedzialność.
- **Dziedziczenie**: `Equipment` i `User` są abstrakcyjne – klasy pochodne (Laptop, Student itd.) rozszerzają je o specyficzne pola. Relacja "jest" uzasadnia użycie dziedziczenia.
- **Interfejsy**: Serwisy komunikują się przez interfejsy (IEquipmentService itp.), co zapewnia luźne sprzężenie i ułatwia ewentualną wymianę implementers (np. na prawdziwą bazę danych).
- **Reguły biznesowe** (limity wypożyczeń, kara) są wyodrębnione w serwisach – łatwo je zmienić, modyfikując jedną stałą lub właściwość.

## Odpowiedzi na pytania
1. **Fast-forward merge** – gdy gałąź `main` nie ma nowych commitów od momentu utworzenia gałęzi.  
   **Merge commit** – gdy obie gałęzie mają własne, rozbieżne commity.
2. **Merge** tworzy dodatkowy commit scalający i zachowuje historię rozgałęzień.  
   **Rebase** przepisuje commity z jednej gałęzi na czubek drugiej, tworząc liniową historię, ale zmienia hashe.
3. **Konflikt** został rozwiązany w pliku `Program.cs` przy zmianie komunikatu powitalnego. Wybrałem wersję z gałęzi `main`, ponieważ była bardziej techniczna.