#pragma once
#include "Arduino.h"

class output_handler
{
public:
  void to_sadm(short &deg);
  void to_test_unit(byte &controller_id, int &data);
private:
  
};

