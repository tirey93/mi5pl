W katalogu znajdują się wyeksportowane pliki z grafikami, czcionkami oraz tłumaczeniem.
Folder podzieliłem na wersję steam i gog ponieważ platformy te wymagają innych plików spolszczeniowych.
Numery podkatalogów odpowiadają kolejnym epizodom gry.
Pliki langdb, zawierające zaszyfrowany tekst tłumaczenia(do odkodowania służy TTGTools) zawierają spolszczenie w starszej wersji. Niniejsze repozytorium ma za zadanie je poprawić wykonując wymaganą korektę lub retranslację.
Katalogi zostały utworzone za pomocą narzędzia ttarchext. Materiałem źródłowym były pliki tłumaczenia znalezione na stronie GPP: https://grajpopolsku.pl/download/tales-of-monkey-island-rise-of-the-pirate-god/

Wpakowywanie za pomocą ttarchext 
Steam
1. .\ttarchext.exe -b -V 7 24 0.ttarch "extracted\steam\1"
2. .\ttarchext.exe -b -x -V 8 26 0.ttarch "extracted\steam\2"
3. .\ttarchext.exe -b -x -V 8 27 0.ttarch "extracted\steam\3"
4. .\ttarchext.exe -b -x -V 8 29 0.ttarch "extracted\steam\4"
5. .\ttarchext.exe -b -x -V 8 31 0.ttarch "extracted\steam\5"

Gog
1. .\ttarchext.exe -b -x -V 8 24 0.ttarch "extracted\gog\1"
2. .\ttarchext.exe -b -x -V 8 26 0.ttarch "extracted\gog\2"
3. .\ttarchext.exe -b -x -V 8 27 0.ttarch "extracted\gog\3"
4. .\ttarchext.exe -b -x -V 8 29 0.ttarch "extracted\gog\4"
5. .\ttarchext.exe -b -x -V 8 31 0.ttarch "extracted\gog\5"