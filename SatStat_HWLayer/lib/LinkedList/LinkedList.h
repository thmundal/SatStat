#pragma once
#include "Node.h"

template <class KEY, class VALUE>
class LinkedList
{
public:
	LinkedList();
	LinkedList(const LinkedList<KEY, VALUE> &other);
	~LinkedList();

	LinkedList<KEY, VALUE> &operator=(const LinkedList<KEY, VALUE> &other);
	VALUE &operator[](int index);
	VALUE& operator[](const KEY& key);

	void add(const KEY &key);
	void append(const KEY &key, const VALUE &value);
	VALUE &get(const KEY &key);
	void set(const KEY &key, const VALUE& value);
	void erase(const KEY &key);
	void setDefault(const VALUE &value);
	int count() const;	

private:
	Node<KEY, VALUE> *m_next;
	VALUE m_default;
	int m_count;
};

template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::LinkedList()
{
	m_next = 0;
	m_count = 0;
}

template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::LinkedList(const LinkedList<KEY, VALUE>& other)
{	
	*this = other;
}

template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::~LinkedList()
{
	if (m_next != 0)
	{
		delete m_next;
	}
}

// Jon later som han skjønner
template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>& LinkedList<KEY, VALUE>::operator=(const LinkedList<KEY, VALUE>& other)
{
	if (m_next != 0)
	{
		delete m_next;
	}

	m_default = other.m_default;
	m_count = other.m_count;

	if (other.m_next != 0)
	{
		m_next = new Node<KEY, VALUE>;
		*m_next = *other.m_next;
	}

	return *this;
}

// Jon tester
template<class KEY, class VALUE>
inline VALUE& LinkedList<KEY, VALUE>::operator[](int index)
{
	if (m_next != 0)
	{
		return m_next->get_index(index, m_default);
	}

	return m_default;
}

// Jon tester
template<class KEY, class VALUE>
inline VALUE& LinkedList<KEY, VALUE>::operator[](const KEY& key)
{
	return get(key);
}

// Jon tester
template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::add(const KEY &key)
{
	if (m_next == 0)
	{
		m_next = new Node<KEY, VALUE>(key);
	}
	else
	{
		m_next->add(key);
	}
	m_count++;
}

template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::append(const KEY &key, const VALUE &value)
{
	if (m_next == 0)
	{
		m_next = new Node<KEY,VALUE>(key, value);
	}
	else
	{
		m_next->append(key, value);
	}
	m_count++;
}

template<class KEY, class VALUE>
inline VALUE &LinkedList<KEY, VALUE>::get(const KEY &key)
{
	if (m_next != 0)
		return m_next->get(key, m_default);
	return m_default;
}

// Jon tester
template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::set(const KEY &key, const VALUE& value)
{
	if (m_next != 0)
	{
		if (m_next->set(key, value))
		{
			m_count++;
		}
	}
}

// Jon tester
template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::erase(const KEY &key)
{	
	if (m_next != 0)
	{
		if (m_next->erase(key, m_next))
		{
			m_count--;
		}
	}
}

template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::setDefault(const VALUE & value)
{
	m_default = value;
}

// Jon tester
template<class KEY, class VALUE>
inline int LinkedList<KEY, VALUE>::count() const
{
	return m_count;
}
