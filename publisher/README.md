Publisher posiada 4 typy czujników:
- temperatura powietrza (air_temp)
- temperatura podłoża (sub_temp)
- wilgotność powietrza (air_hum)
- wilgotność podłoża (sub_hum)

Każdy z typów ma po 30 czujników.

Czujniki znajdują się w szklarni pomidorów. 
Czujniki pilnują, aby warunki były idealne dla wzrostu pomidorów.
Na początku wartości losowane są z przedziału idealnych wartości, a następnie na bieżąco zmieniane o -1, 0 i 1 z prawdopodobieństwem największego trafienia w 0.

Kolejka jest deklarowana na początku z nazwą sensors-queue, bindowana jest z exchangem.
Na bieżąco publikowane są trwałe wiadomości z id sensora, wartością sensora i datą.

