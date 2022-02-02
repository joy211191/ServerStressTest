
                FROM ubuntu:latest
                RUN apt-get update -qq; \
                apt-get install -qq -y curl \
                    && apt-get install -y sudo \
                    && apt-get clean \
                    && rm -rf /var/lib/apt/lists/*
                COPY Server server
                RUN useradd -ms /bin/bash unity
                RUN adduser unity sudo
                RUN echo '%sudo ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers
                RUN chown unity:unity -R server/server.x86_64
                RUN chmod -R 755 server
                USER unity
                CMD ["/bin/bash", "-c", "exec ./server/server.x86_64 -batchmode -nographics 2>&1 | (sudo tee -a /aci/logs/unity_server.log)"]
            