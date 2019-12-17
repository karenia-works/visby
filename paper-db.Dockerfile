FROM postgres:12  As base
Workdir /usr/share/postgresql/12
copy ["extent/pg_jieba--1.1.1.sql","extension/"]
copy ["extent/pg_jieba.control","extension/"]
# Run   ["mkdir","tsearch_data"]
copy ["extent/tsearch_data/jieba_base.dict","tsearch_data/"]
copy ["extent/tsearch_data/jieba_hmm.model","tsearch_data/"]
copy ["extent/tsearch_data/jieba_user.dict","tsearch_data/"]
copy ["extent/tsearch_data/jieba.stop","tsearch_data/"]
copy ["extent/tsearch_data/jieba.idf","tsearch_data/"]
Workdir /usr/lib/postgresql/12/
copy ["extent/pg_jieba.so","lib/"]