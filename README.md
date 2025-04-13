# MealPlanner
Author: KrystylGP

Beskrivning: En applikation som är menad till att hjälpa planera måltider i olika tidsram. Huvudsyftet är att hålla koll på inventering av ingredienser man har hemma samt kolla vad man planerat laga. Planeringen kommer kalkylera ingredienser man har hemma och de ingredienser som saknas hamnar i en GroceryList. Applikationen har stora rum för utökningar, se längst ner i ReadMe.

Huvud entiteter: Ingredient, Meal, MealPlanner

Applikationen är byggd med ASP .NET MVC, därav arkitektur lyder: frontend tar dig till en metod till korrekt Controller som hanterar bl.a. valideringar av data, sedan skickar Controllern datat till Service som bearbetar datat (Business Logic). Service kallar på motsvarande metod till Repository lagret eller databaslagret som hanterar lagring av datat.

Demo bilder (alla valideringar + detaljer ej med) nedan:

![image](https://github.com/user-attachments/assets/79fb79d9-c5a4-4c6c-8b1d-df737976b16c)
Skapa ingredienser kräver namn och typ.

![image](https://github.com/user-attachments/assets/abeddbd6-1e56-48e0-8b7b-3575d68bc10a)
Lista på alla måltider.

![image](https://github.com/user-attachments/assets/8acf0563-ba3d-4ab3-83b9-2fd0a1f9015b)
Användarens måltidsplaner med ingående måltider.

![image](https://github.com/user-attachments/assets/1514ae49-cd81-4a83-93c9-09a41f588742)
Att handla lista.


Utökningar jag hade velat implementera: 
* Kaloriräkning
* Bilder på färdig måltid
* Styling (css)
* Recept
* Admin funktion som kan t.ex. redigera och ta bort måltider, ingredienser mm.
* Djupare detalj i måltidsplanering, ex. måndag laga si och tisdag laga så.
* Historik om vad man redan lagat för att kunna laga andra måltider.
