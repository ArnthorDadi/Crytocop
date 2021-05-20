# set base image (host OS)
FROM python:3.8

# copy the content of the local src directory to the working directory
COPY . /app

# set the working directory in the container
WORKDIR /app

# copy the dependencies file to the working directory
COPY requirements.txt .

# install dependencies
RUN pip install -r requirements.txt

# command to run on container start
CMD [ "python", "./email_service.py" ]