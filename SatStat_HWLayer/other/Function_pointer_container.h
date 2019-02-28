#pragma once

class Function_pointer_container
{
public:		
	template <class R, class P0 = void, class... P> struct Gfp;
};

template <class R, class P0 = void, class... P>
struct Function_pointer_container::Gfp
{
	R(*func)(P...);
};

template <class R>
struct Function_pointer_container::Gfp <R, void>
{
	R(*func)(void);
};