#pragma once
#include "Json_container.h"
#include "../../lib/SSTL/sstl.h"

/**
*	Abstract class with all static members for the methods to be compatible with list insertion.
*	This class is responsible for handling all the requests listed in the SatStat communication protocol.
*/
class Request_functions
{
public:
	/**
	*	Pure virtual destructor to make class abstract.
	*/
	virtual ~Request_functions() = 0;

	static void subscribe(Json_container<JsonObject>& request);
	static void unsubscribe(Json_container<JsonObject>& request);

	static void reset();
};