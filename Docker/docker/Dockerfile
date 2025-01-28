FROM ubuntu
RUN apt-get update && apt-get install -y gcc \
    gdb \
    vim \
    nano \
    bsdmainutils \
    make \
    python3 \
    python3-pip \
    wget \
    curl \
    software-properties-common \
    apt-transport-https \
    ca-certificates \
    gnupg \
    lsb-release \
    xserver-xorg-video-dummy \
    openjdk-17-jdk \ 
    openjfx \
    --no-install-recommends

RUN pip3 install pandas zucchini openpyxl --break-system-packages

# quiets some warnings
ENV GIT_PYTHON_REFRESH=quiet

# Clean up unnecessary files
RUN apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*


