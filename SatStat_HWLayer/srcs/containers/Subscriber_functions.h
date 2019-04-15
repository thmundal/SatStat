#pragma once
#include "Arduino.h"
#include "Json_object_container.h"
#include "../../libraries/SSTL/sstl.h"

class Subscriber_functions
{
public:
	virtual ~Subscriber_functions() = 0;

	static void subscribe(Json_container<JsonObject>* request);
	static void unsubscribe(Json_container<JsonObject>* request);
};