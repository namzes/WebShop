## ✅ = Klart | ⬜ = Ej klart

---

## 🛍️ Webbutikens funktioner

- [ ] Applikationen har en startsida som visar en lista av produkter.
- [ ] Applikationen innehåller minst 10 produkter.
- [ ] Varje produkt har ett unikt ID, namn, beskrivning, bild-URL och pris.
- [ ] Produkterna visas upp med hjälp av Razor-komponenter på startsidan.
- [ ] Produktkomponenterna visar endast en översikt av produktdetaljer, inte all information.
- [ ] Varje produktkomponent innehåller en knapp för att lägga till produkten i varukorgen.
- [ ] Klick på en produktkomponent leder till en produktsida med mer information.
- [ ] Produktsidan visar alla detaljer för den valda produkten.
- [ ] På produktsidan finns en knapp för att lägga till produkten i varukorgen.
- [ ] Man kan navigera till en produktsida via webbadressen (t.ex. `localhost/product/1`).
- [ ] En varukorgssida är tillgänglig via en knapp eller länk.
- [ ] Varukorgssidan visar alla produkter som lagts i varukorgen.
- [ ] Varukorgssidan innehåller ett formulär för att fylla i adressuppgifter.
- [ ] När formuläret skickas in omdirigeras användaren till en bekräftelsesida.
- [ ] Bekräftelsesidan visar köpta produkter samt användarens namn och adressuppgifter.
- [ ] Varukorgen töms efter att köpet är genomfört.

---

## 🛠️ Kodstruktur och uppdelning

- [ ] Minst två komponenter används (förutom sidor och Layout-komponenter).
- [ ] All data i "Godkänt"-scenariot kan vara statisk (hårdkodad).
- [ ] Blazor Web App-mallen används.
- [ ] HTML används korrekt och valideras.
- [ ] Semantiska HTML-element används där det är möjligt.
- [ ] CSS är tydligt strukturerad och bidrar till en bra användarupplevelse.
- [ ] Applikationen är responsiv och anpassad för både mobil och desktop.
- [ ] Inga CSS-ramverk (t.ex. Bootstrap, Tailwind) används.

---

## 🏆 Krav för Väl Godkänt

- [ ] Applikationen använder både Server-rendering och Client-side rendering.
- [ ] Minst fyra komponenter används (förutom sidor och Layout-komponenter).
- [ ] Produkternas kvantiteter hanteras i systemet.
- [ ] Produkter kan bli slutsålda.
- [ ] Produktkomponenterna ändrar utseende när en produkt är slutsåld eller på rea.
- [ ] Produktsidan visar priset i olika valutor med hjälp av API:et https://apininjas.com/api/exchangerate.
- [ ] API-nyckeln för valutakonvertering är säker och kan inte nås av slutanvändaren.
- [ ] En databas och ett REST API används för att lagra och hämta produktdata.
- [ ] Produktdatan lagras i databasen (dock inte valutakurserna).

---

## 🔐 Användarhantering och persistens

- [ ] Användare kan registrera ett konto med användarnamn och lösenord.
- [ ] Användare kan logga in med sitt användarnamn och lösenord.
- [ ] Varukorgen kommer ihåg vilka produkter som användaren har lagt till vid inloggning.
- [ ] Ett köp kan inte genomföras utan att användaren är inloggad.
- [ ] Informationen hämtas från servern via ett HTTP-anrop till en API-endpoint.
- [ ] Delade klasser används i både frontend och backend.

---

## 📁 Projektstruktur

- [ ] Applikationen är uppdelad i tre projekt:
  - `WebshopFrontend`: UI, sidor, komponenter
  - `WebshopBackend`: API-endpoints, affärslogik
  - `WebshopShared`: Delade modeller, DTOs, valideringslogik

---

## 📝 Dokumentation

- [ ] En fil `Analysis.md` finns i projektets rotkatalog.
- [ ] `Analysis.md` dokumenterar hur .NET Core:s felsökningsverktyg användes för att identifiera och åtgärda minst en bugg.
- [ ] Felsökningsprocessen innehåller en förklaring samt en reflektion.
- [ ] En fil `Assignment.md` finns som listar alla kriterier och har kryssrutor för att markera avklarade uppgifter.
