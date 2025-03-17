## ✅ = Klart | ⬜ = Ej klart

---

## 🛍️ Webbutikens funktioner

- [x] Applikationen har en startsida som visar en lista av produkter.
- [x] Applikationen innehåller minst 10 produkter.
- [x] Varje produkt har ett unikt ID, namn, beskrivning, bild-URL och pris.
- [x] Produkterna visas upp med hjälp av Razor-komponenter på startsidan.
- [x] Produktkomponenterna visar endast en översikt av produktdetaljer, inte all information.
- [x] Varje produktkomponent innehåller en knapp för att lägga till produkten i varukorgen.
- [x] Klick på en produktkomponent leder till en produktsida med mer information.
- [x] Produktsidan visar alla detaljer för den valda produkten.
- [x] På produktsidan finns en knapp för att lägga till produkten i varukorgen.
- [x] Man kan navigera till en produktsida via webbadressen (t.ex. `localhost/product/1`).
- [ ] En varukorgssida är tillgänglig via en knapp eller länk.
- [ ] Varukorgssidan visar alla produkter som lagts i varukorgen.
- [ ] Varukorgssidan innehåller ett formulär för att fylla i adressuppgifter.
- [ ] När formuläret skickas in omdirigeras användaren till en bekräftelsesida.
- [ ] Bekräftelsesidan visar köpta produkter samt användarens namn och adressuppgifter.
- [ ] Varukorgen töms efter att köpet är genomfört.

---

## 🛠️ Kodstruktur och uppdelning

- [x] Minst två komponenter används (förutom sidor och Layout-komponenter).
- [ ] HTML används korrekt och valideras.
- [ ] Semantiska HTML-element används där det är möjligt.
- [ ] CSS är tydligt strukturerad och bidrar till en bra användarupplevelse.
- [x] Applikationen är responsiv och anpassad för både mobil och desktop.
- [x] Inga CSS-ramverk (t.ex. Bootstrap, Tailwind) används.

---

## 🏆 Krav för Väl Godkänt

- [x] Minst fyra komponenter används (förutom sidor och Layout-komponenter).
- [x] Produkternas kvantiteter hanteras i systemet.
- [ ] Produkter kan bli slutsålda.
- [ ] Produktkomponenterna ändrar utseende när en produkt är slutsåld eller på rea.
- [ ] Produktsidan visar priset i olika valutor med hjälp av API:et https://apininjas.com/api/exchangerate.
- [ ] API-nyckeln för valutakonvertering är säker och kan inte nås av slutanvändaren.
- [x] En databas och ett REST API används för att lagra och hämta produktdata.
- [x] Produktdatan lagras i databasen (dock inte valutakurserna).

---

## 🔐 Användarhantering och persistens

- [x] Användare kan registrera ett konto med användarnamn och lösenord.
- [x] Användare kan logga in med sitt användarnamn och lösenord.
- [ ] Varukorgen kommer ihåg vilka produkter som användaren har lagt till vid inloggning.
- [ ] Ett köp kan inte genomföras utan att användaren är inloggad.
- [ ] Informationen hämtas från servern via ett HTTP-anrop till en API-endpoint.
- [ ] Delade klasser används i både frontend och backend.

---

## 📁 Projektstruktur

- [x] Applikationen är uppdelad i tre projekt:
  - `WebshopFrontend`: UI, sidor, komponenter
  - `WebshopBackend`: API-endpoints, affärslogik
  - `WebshopShared`: Delade modeller, DTOs, valideringslogik

---

## 📝 Dokumentation

- [ ] En fil `Analysis.md` finns i projektets rotkatalog.
- [ ] `Analysis.md` dokumenterar hur .NET Core:s felsökningsverktyg användes för att identifiera och åtgärda minst en bugg.
- [ ] Felsökningsprocessen innehåller en förklaring samt en reflektion.
- [ ] En fil `Assignment.md` finns som listar alla kriterier och har kryssrutor för att markera avklarade uppgifter.
