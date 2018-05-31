# GIZ
Graphs project

Projekt zrealizowano w języku C#.

Opis rozwiązania:
1. Zamiana danych wejściowych na graf: 
  * Wierzchołkami grafu są państwa
  * Pomiędzy dwoma wierzchołkami istnieje krawędź nieskierowana <=> państwa sąsiadują ze sobą
  * Waga krawędzi odpowiada sumie powierzchni łączonych państw (koszt przejścia pomiędzy nimi)
2. Ponieważ niemożliwe jest istnienie krawędzi o wagach ujemnych, problem rozwiązujemy za pomocą algorytmu Dijkstry
