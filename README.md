# ToDoList.cs
ToDoList - ToDoLy
ToDoLy är ett konsolprogram i C# för att hantera att göra-uppgifter. Du kan lägga till, visa, redigera och spara uppgifter. Alla uppgifter sparas i en lokal fil (`tasks.json`) så att de finns kvar nästa gång du startar programmet.

## Funktioner

- Lägg till nya uppgifter med:
  - Titel
  - Förfallodatum (YYYY-MM-DD)
  - Projekt (valfritt)
- Lista alla uppgifter i en tabell
- Redigera uppgifter:
  - Uppdatera titel, datum eller projekt
  - Markera som klar
  - Ta bort uppgift
- Uppgifterna sparas automatiskt i `tasks.json`

## Så här kör du programmet

1. Klona eller ladda ner projektet:

   ```bash
   git clone https://github.com/VictoriaGleam/todoly.git
   cd todoly
