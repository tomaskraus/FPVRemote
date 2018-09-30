int incomingByte = 0;   // for incoming serial data



void setup() {
  Serial.begin(9600);     // opens serial port, sets data rate to 9600 bps

  // initialize digital pin LED_BUILTIN as an output.
  pinMode(LED_BUILTIN, OUTPUT);
}

void loop() {

  // send data only when you receive data:
  if (Serial.available() > 0) {
    // read the incoming byte:
    incomingByte = Serial.read();

    if (incomingByte != 10) {
      if (incomingByte > 50) {
        digitalWrite(LED_BUILTIN, HIGH);   // turn the LED on (HIGH is the voltage level)
      } else {
        digitalWrite(LED_BUILTIN, LOW);   // turn the LED on (HIGH is the voltage level)
      }
    }

    // say what you got:
    // Serial.print("I received: ");
    // Serial.println(incomingByte, DEC);
  }
}
