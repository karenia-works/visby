FROM postgres:12
RUN apt-get update 
RUN apt-get install git make libpq-dev libpq5 -y -V
WORKDIR /tmp/jieba
RUN git clone https://github.com/jaiminpan/pg_jieba.git --recursive --depth=5 --shallow-submodules
RUN apt-get install cmake gcc g++ -y -V
WORKDIR /tmp/jieba/pg_jieba
RUN mkdir build && cd build && cmake .. -DPostgreSQL_TYPE_INCLUDE_DIR=/usr/include/postgresql/12/server
WORKDIR /tmp/jieba/pg_jieba/build
RUN make 
RUN make install

