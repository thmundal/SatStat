#pragma once
#include "Arduino.h"
#include "Json_container.h"

class Json_object_container : public Json_container<JsonObject>
{
public:
	Json_object_container();

	void create();
	void parse(const String &json);
};