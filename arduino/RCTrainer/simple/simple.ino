int incomingByte = 0;   // for incoming serial data

const int MaxChars = 8; //few bytes more than signed short value
char strValue[MaxChars + 1];
int index = 0;
long x = 0; //4 bytes

void setup() {
  Serial.begin(9600);     // opens serial port, sets data rate to 9600 bps

  // initialize digital pin LED_BUILTIN as an output.
  pinMode(LED_BUILTIN, OUTPUT);
}

void loop() {
}


void serialEvent()
{
  while (Serial.available())
  {
    char ch = Serial.read();
    if (index < MaxChars && (isDigit(ch) || ch == '-')) {
      strValue[index++] = ch;
    } else {
      strValue[index] = 0;
      x = atol(strValue);
      if (x > 2000) {
        digitalWrite(LED_BUILTIN, HIGH);
      } else {
        digitalWrite(LED_BUILTIN, LOW);
      }
      index = 0;
    }
  }
}
