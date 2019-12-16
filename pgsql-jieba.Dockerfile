FROM postgres:12 AS BASE
RUN apt-get update 
RUN apt-get install git make libpq-dev -y -V
WORKDIR /tmp/jieba
RUN git clone https://github.com/jaiminpan/pg_jieba.git --recursive --depth=5 --shallow-submodules
RUN apt-get install cmake -y -V
WORKDIR /tmp/jieba/pg_jieba
RUN mkdir build && cd build && cmake ..
WORKDIR /tmp/jieba/pg_jieba/build
RUN make 
RUN make install

