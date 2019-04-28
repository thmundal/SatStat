#pragma once
#include "Arduino.h"
#include "Json_container.h"
#include "../../lib/SSTL/sstl.h"

class Subscriber_functions
{
public:
	virtual ~Subscriber_functions() = 0;

	static void subscribe(Json_container<JsonObject>& request);
	static void unsubscribe(Json_container<JsonObject>& request);
};