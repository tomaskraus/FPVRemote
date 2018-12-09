# RC Auto + FPVRemote App \(rychlý návod)

## Seznam zařízení

### Pohonná baterie (1.1)
- NiMh
  - vysoké samovybíjení
    - nabíjet max. den před použitím pro optimální výsledky
  - vysoký paměťový efekt! Nepřerušovat nabíjení, není-li zcela nabitá. Jinak ztratí část své kapacity.
  - Při nabíjení se se může dost zahřívat (60°C je v normě).
  - ! Při náročném použití (ostrá jízda) se může silně zahřívat (okolo 100°C)
  - před nabíjením nechte baterii vychladnout na pokojovou teplotu
  - při použití baterie venku v teplotách kolem bodu mrazu je dobré baterii zahřát (na 30°C a více)
  - konektor baterie (1.1.1)
    - konektor má na sobě západku. Tu je nutné stisknout při odpojování baterie.
  - ! Při odpojování baterii držte vždy za konektor. Odpojování taháním za kabel může vést k sundání konektoru a zkratování vodičů s rizikem požáru!

### základní nabíječka NiMh baterií (1.2)
- pomalé nabíjení
- doba nabití 1800mAh baterie: cca. 6h
- nutno vypnout ze sítě po přesné době nabíjení, jinak hrozí přebití

### rychlá nabíječka NiMh baterií (1.3)
- Vysoký nabíjecí proud: 3A
  - nepoužívat pro baterie kapacity menší než 3000mAh, jinak hrozí rychlé zničení baterie
- doba nabíjení 3300mAh: cca. 1h
- automatické ukončení nabíjení, až je baterie nabitá
- nabito je, když svítí zelená dioda

### FPV baterie (2.1)
- silový konektor (2.1.1)
- servisní/balanční konektor (2.1.2)
  - vývody jednotlivých článků baterie 
- LiPol technologie, 2 články x 3.7V, 850mAh
- nízké samovybíjení
  - nabitá vydrží měsíce, ale doporučuje se baterii vybít na udržovací napětí, bude-li pár týdnů nepoužívaná
- ! Potenciálně nebezpečná!
  - video (propíchnutí malé LiPol): https://www.youtube.com/watch?v=esrp1IzPjSQ
  - vyžaduje zvýšenou opatrnost při nabíjení a skladování, riziko požáru
  - obsahuje lithium, nejde hasit vodou
- jednotlivé články baterie se nesmí v napětí lišit více než o 0,1V
  - jinak se baterie může zničit, s rizikem požáru
- Baterie se může stářím nebo nesprávným použitím nafouknout
  - nepropichujte nafouknutou baterii! jinak se baterie může zničit, s rizikem požáru!
- před nabíjením nechte baterii vychladnout na pokojovou teplotu
- při použití baterie venku v teplotách kolem bodu mrazu je dobré baterii zahřát (na 30°C a více)
- ! Při odpojování baterii držte vždy za konektor. Odpojování taháním za kabel může vést k sundání konektoru a zkratování vodičů s rizikem požáru!

### nabíječka FPV baterie (2.2)
- nabíječka LiPol baterií, s balancerem a odděleným síťovým adaptérem
- doba nabíjení 800mAh baterie: cca. 1h
- nabito je, když přestane svítit červená dioda


### RC Auto (3)
RC auto, upravené pro přenos obrazu FPV (First Person View)
- konektor pro pohonnou baterii (3.1)
- RC přijímač (3.2)
  - přijímá signál z vysílačky a ovládá auto (rychlost, zatáčení atd.)
- regulátor (3.3)
- motor (3.4)
- servo řízení (3.5)
- dok pro FPV baterii (3.6)
  - chrání před nárazy a izoluje

### FPV stojan (4)
Funkční celek přidaný k autu, zajišťuje funkci FPV.
  - FPV kamera (4.1)
    - funguje i za nízkého osvětlení: přepne se na černobílý obraz
    - na přenášeném obrazu zobrazuje čas od zapnutí kamery a aktuální napájecí napětí
    - při nízkém napětí indikátor napětí bliká
      - skončete jízdu a co nejdříve nabijte FPV baterii, abyste předešli poškození FPV baterie
    - krytka objektivu (4.2)
  - držák krytky kamery (4.3)
    - opatření proti ztrátě krytky kamery
  - deformační prvek (4.4)
    - vyměnitelný díl z překližky, přišroubovaný k FPV stojanu. Při kolizi FPV stojanu s překážkou se stojan zlomí v místě překližky a ochrání tak podvozkovou skupinu, ke keré je připevněn, od poškození.
  - FPV vysílač (4.5)
    - přišroubovaná FPV anténa (4.6) 
    - napájecí konektor FPV vysílače (4.7)
      - připojte k FPV baterii jen po dobu provozu, vysílač se silně zahřívá!
      - nezapíntejte FPV vysílač bez antény, dojde k přehřátí a zničení FPV vysílače
  - nouzový silový konektor (4.8)
    - napájení z pohonné baterie auta, připojit k napájecímu konektoru FPV (4.7)
    - použijte, když není k dispozici FPV baterie
    - při velké zátěži pohonné baterie (např. prudká akcelerace) nestačí napájet FPV a objeví se výpadky obrazu


### Vysílačka (5)
Slouží ke standardnímu ovládání auta. Pomocí kabelů a enkodéru se připojí k PC a pak se dá vysílačka z PC ovládat.
- vypínač (5.1)
- přepínače (5.2.1 - 5.2.4)
- páky ovládání (5.3)
  - levá (5.3.1)
  - pravá (5.3.2)
- tlačítka menu (5.5)
  - up (5.5.1), down (5.5.2)
  - ok (5.5.3), cancel (5.5.4)
    - je rozeznáván krátký a dlouhý stisk
- ostatní ovládací prvky
  - potenciometry (5.6.1, 5.6.2)
    - nepoužito pro FPV Auto
  - trimry (5.7.1 - 5.7.4)
    - trimr pro nastavení středové polohy zatáčení (5.7.4)
- trainer port (5.8)
  - konektor na zadní straně vysílačky
- šachta pro baterie
  - 4xAA, možné použít i nabíjecí (NiCd, NiMh)

Po několika minutách nečinnosti začne zapnutá vysílačka pípat. Stiskněte tlačítko "cancel" na vysílačce, vysílačka pípat přestane.

### Enkodér (6)
Převádí komunikaci z usb portu PC do formátu srozumitelného vysílačce. Připojuje se k PC.
- usb kabel k PC (6.1)
  - kabel mini usb
- kabel k vysílačce (6.2)
  - kabel jack-PS2
### FPV přijímač (7)
- přijímá obrazový signál z auta do PC. Připojuje se k PC. 
  - zapojte k PC jen po dobu používání, poměrně hodně se zahřívá
- usb kabel k PC (7.1)
  - kabel mikro usb
### gamepad (8)
PC joystick pro alternativní ovládání auta z PC. Připojuje se k PC.
- není nutný k provozu
