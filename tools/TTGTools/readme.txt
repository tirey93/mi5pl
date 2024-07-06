Narzędzie zostało wgrane w formie releasu.
Link do releasu:
https://github.com/tirey93/mi5pl/releases/tag/TTGTools

Link do oryginalnego releasu: 
https://gitflic.ru/project/pashok6798/ttg-tools

Moje wskazówki
Na początku należy ustawić config. Wskazujemy ścieżki do katalogów input, output, temp. Na końcu wskazujemy dokładną ścieżkę do programu ttarchext.exe.

Odszyfrowanie pliku langdb 
1. Wstawiamy oryginalny plik .langdb to katalogu input
2. Wybieramy Auto (De)Packer -> Decrypt,Export
3. Odszyfrowany plik .txt pojawia się w katalogu output

Zaszyfrowywanie do pliku langdb
Po zakończeniu tłumaczenia mamy plik .po w katalogu OmegaT/target. 
1. Należy go konwertować do plik .txt za pomocą narzędzia po2tomi_converter
2. Plik .txt wraz z oryginalnym plikim .langdb umieszczamy w input
3. Wybieramy Auto (De)Packer -> Encrypt, Pack, Import
4. Zaszyfrowany plik .langdb z naszym tłumaczeniem znajduje się w katalogu output
