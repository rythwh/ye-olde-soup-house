# Ye Olde Soupe House

#### Description
A proof-of-concept game made in about 12 hours. A variation on restaurant cooking games, in **Ye Olde Soupe House** you cook various types of soups for customers as they request it. If you take too long they will leave in anger, costing you the price of the ingredients. Successfully serve them however, and you'll make a nice profit!

Experimenting with interface design was the primary motivation for the project. I wanted to make a game where the vast majority of the interactions and information was given through non-text cues and to prevent the use of traditional interface "windows" where possible (e.g. the recipe book tries to look like an actual book with pages rather than a panel with a scroll rect, or customers turn red to show anger rather than an "anger bar" above their heads).

Certain primary interactions or critical information was difficult to show without text or buttons however, such as the controls for the cooking station or the name of the dish above customer's heads.

#### Screenshots (with captions)

> **Main Menu**
> ![Main Menu](/Screenshots/main-menu.png)
> *Main menu shown when you start the game.*

> **Restaurant Interface**
> ![Restaurant Interface](/Screenshots/initial-game-screen.png)
> *Primary interface shown when you begin playing the game. Customers enter along the top, cooking station and recipe book are in the middle, with ingredients to either side.*

> **Recipe Book**
> ![Recipe Book](/Screenshots/recipe-book.png)
> *Recipe book shown when you click on the book from the restaurant interface. Shows each type of soup along with the ingredients to make it. Clicking the arrows to the left/right allow flipping the page to see more recipes.*

> **Cooking (Tomato) Soup**
> ![Cooking (Tomato) Soup](/Screenshots/cooking-tomato-soup.png)
> *Clicking the ingredients on the left/right add them into the pot. You can then begin cooking by clicking **Light Fire** or stop by clicking **Extinguish** below the cooking station. Additionally, if you made a mistake, you can click **Dump** which will empty the pot but waste the ingredients. Customers will become angrier (and redder) the longer they wait, eventually giving up and leaving (e.g. the person requesting the Potato and Broccoli soup is almost ready to leave).*

> **Serving (Tomato) Soup**
> ![Serving (Tomato) Soup](/Screenshots/serving-tomato-soup.png)
> *While cooking, the ingredients will slowly turn into the final soup. The soup won't begin cooking until the pot reaches the soup's required temperature (indicated by the orange-ness of the pot and wood). Each soup has a different cooking length and temperature. Once the soup is finished cooking, the fire will be automatically extinguished and you will have the **Serve** option to give the soup to the first customer who requested it and get paid in exchange.*

> **Served (Tomato) Soup**
> ![Served (Tomato) Soup](/Screenshots/served-tomato-soup.png)
> *Once the fire has been extinguished, the pot will immediately begin cooling, so the faster you can get the next ingredients into the pot and cooking, the less time will be spent waiting for the pot to heat back up. Soup can be made in any order, the first customer to request a specific soup will take the first order (FIFO per soup). Additionally, based on the money in the top-right, we started with 300 Gold, spent 90 Gold on ingredients, and made 134 Gold in revenue for a 44 Gold profit. Customers pay more if they are served sooner.*

#### Ideas for Future Development

* Adding more information on prices of ingredients.
* Showing cooking times or various steps in recipes (rather than just add all ingredients and cook).
* Setting and changing prices of dishes and ingredients (supply/demand).
* World interactions (e.g. trader didn't show up so x-soup is more expensive today).
* More types of soups.
* Items to spend money on outside of ingredients (i.e. items to attract customers, improvements to restaurant).
* Difficulty curve.
