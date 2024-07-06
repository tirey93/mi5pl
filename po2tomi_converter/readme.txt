Program ma za zadanie dokonać konwersji pliku .po, obsługiwanego przez OmegaT, do odszyfrowanych plików .txt niezbędnych do wgrania przetłumaczonego tekstu do gry.
Pliki .txt należy zaszyfrować do formatu .langdb za pomocą narzędzia TTGTools, a następnie spakować do archiwum ttarch za pomocą narzędzia ttarchext.

Konfiguracja programu
Plik konfiguracyjny appsettings.json zawiera w sobie elementy niezbędne do poprawnego działania programu. Poniżej opis elementów:

ToPoConversion - ustawiany na true lub false. Domyślnie false. W przypadku normalnego użytkowania programu nie należy tej flagi zmieniać. Jeśli z jakiegoś powodu niezbędne będzie działanie programu w sposób odwrotny(tj. z odszyfrowanych plików gry wygenerować plik .po) to wtedy należy tę wartość ustawić na true.
PoFileLocation - Lokalizacja pliku .po utworzonego w OmegaT z katalogu target. Najlepiej ustawić sobie dokładną ścieżkę do projektu OmegaT/target/tomi.po a program będzie czytał dane z pliku .po od razu po zapisaniu w OmegaT.
TomiEngFileLocation - Lokalizacja odszyfrowanego pliku angielskiego. Znajduje się on w repozytorium w katalogu skrypty/X/english.txt. Najwygodniej jest sobie ustawić ścieżkę do tego miejsca i zapomnieć o sprawie.
TomiPlFileLocation - Lokalizacja odszyfrowanego pliku polskiego. Po uruchomieniu programu w celu wgrania tłumaczenia z pliku .po do gry należy ten plik zaszyfrować do formatu .langdb za pomocą narzędzia TTGTools, a następnie spakować do archiwum ttarch za pomocą narzędzia ttarchext. Znajduje się on w repozytorium w katalogu skrypty/X/polish.txt. Najwygodniej jest sobie ustawić ścieżkę do tego miejsca i zapomnieć o sprawie. 