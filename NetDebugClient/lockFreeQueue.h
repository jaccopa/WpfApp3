#pragma once
#include<atomic>
#include<utility>
#include<stdexcept>
#include<thread>
#include<iostream>
#include<vector>

template <typename T, size_t max_size = 20>
class CLockFreeQueue {
	T queue[max_size];
	std::atomic<size_t> _size;
	const size_t _maxs;
	size_t _read_pos;
	size_t _write_pos;
public:
	//constructor
	CLockFreeQueue()
		: _maxs(max_size), _size(0), _read_pos(0), _write_pos(0)
	{}

	void push(const T& item) {
		if (_size.load() == _maxs) {
			//std::cout << "queue is full" << std::endl;
			//throw exception
			return;
		}
		queue[_write_pos] = item;
		_write_pos = (_write_pos + 1) % _maxs;
		_size.fetch_add(1);
	}

	T& front() {
		if (_size.load() > 0) {
			return  queue[_read_pos];
		}
	}

	void pop() {
		if (_size.load() > 0) {
			_read_pos = (_read_pos + 1) % _maxs;
			_size.fetch_sub(1);
		}
	}

	size_t size() {
		return _size.load();
	}

	bool full() {
		return _size.load() == _maxs;
	}

	bool empty() {
		return _size.load() == 0;
	}

};