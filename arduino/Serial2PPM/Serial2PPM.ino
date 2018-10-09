/*
 * This code can be used to control an RC transmitter from Arduino.
 * Reads a serial input and sends a PPM to the output.
 * Tested with FLYSKY FS-I6 using its trainer port.
 * 
 * Change the configuration section to match your TX's PPM attributes
 * 
 * 2018 Tomas Kraus
 * This is the part of FPVRemote project:
 *   https://github.com/tomaskraus/FPVRemote
 * 
   This code uses the following:

   PPM generator originally written by David Hasko
   on https://code.google.com/p/generate-ppm-signal/
*/

//////////////////////CONFIGURATION///////////////////////////////
#define CHANNEL_NUMBER 4  //set the number of channels
#define CHANNEL_DEFAULT_VALUE 1500  //set the default servo value
#define CHANNEL_MIN_VALUE 1000
#define CHANNEL_MAX_VALUE 2000
#define FRAME_LENGTH 20000  //set the PPM frame length in microseconds (1ms = 1000µs)
#define PULSE_LENGTH 400  //set the pulse length
#define onState 1  //set polarity of the pulses: 1 is positive, 0 is negative
#define sigPin 10  //set PPM signal output pin on the arduino

/*this array holds the servo values for the ppm signal
  change theese values in your code (usually servo values move between 1000 and 2000)*/
volatile int ppm[CHANNEL_NUMBER];


int chan = 1;

const int MaxDigits = 3;
char strValue[MaxDigits + 1];
int index = 0;
int x = 0;


void setup() {

  //pinMode(LED_BUILTIN, OUTPUT);


  // ---- PPM INITIALIZATION -------------------------------------------------------------
  // initiallize default ppm values
  for (int i = 0; i < CHANNEL_NUMBER; i++) {
    ppm[i] = CHANNEL_DEFAULT_VALUE;
  }
  // shut down a motor
  ppm[2] = CHANNEL_MIN_VALUE;
  
  // ---- PPM generator --------------------------------------------------
  pinMode(sigPin, OUTPUT);
  digitalWrite(sigPin, !onState);  // set the PPM signal pin to the default state (off)

  cli();
  TCCR1A = 0; // set entire TCCR1 register to 0
  TCCR1B = 0;

  OCR1A = 100;  // compare match register, change this
  TCCR1B |= (1 << WGM12);  // turn on CTC mode
  TCCR1B |= (1 << CS11);  // 8 prescaler: 0,5 microseconds at 16mhz
  TIMSK1 |= (1 << OCIE1A); // enable timer compare interrupt
  sei();


  // ---- OTHER INITIALIZATION -------------------------------------------------------------

  // initialize serial:
  Serial.begin(9600);

  //digitalWrite(LED_BUILTIN, LOW);

}

void loop() {

  /*
    Here modify ppm array and set any channel to value between 1000 and 2000.
    Timer running in the background will take care of the rest and automatically
    generate PPM signal on output pin using values in ppm array
  */

}

ISR(TIMER1_COMPA_vect) { //leave this alone
  static boolean state = true;

  TCNT1 = 0;

  if (state) {  //start pulse
    digitalWrite(sigPin, onState);
    OCR1A = PULSE_LENGTH * 2;
    state = false;
  } else { //end pulse and calculate when to start the next pulse
    static byte cur_chan_numb;
    static unsigned int calc_rest;

    digitalWrite(sigPin, !onState);
    state = true;

    if (cur_chan_numb >= CHANNEL_NUMBER) {
      cur_chan_numb = 0;
      calc_rest = calc_rest + PULSE_LENGTH;//
      OCR1A = (FRAME_LENGTH - calc_rest) * 2;
      calc_rest = 0;
    }
    else {
      OCR1A = (ppm[cur_chan_numb] - PULSE_LENGTH) * 2;
      calc_rest = calc_rest + ppm[cur_chan_numb];
      cur_chan_numb++;
    }
  }
}

// --------------------------------------------------------------------------------------

void serialEvent()
{
  while (Serial.available())
  {
    char ch = Serial.read();
    if (index < MaxDigits && isDigit(ch)) {
      strValue[index++] = ch;
    } else {
      strValue[index] = 0;
      x = atoi(strValue);
      x = map(x, 0, 255, 1000, 2000);
      //Serial.println(x);
      ppm[chan] = x; 
      index = 0;
    }
  }
}
