# Use the official Python 3.12 slim image as the base
FROM python:3.12-slim

# Set environment variables
ENV PYTHONUNBUFFERED=1
ENV APP_HOME=/app

# Set the working directory
WORKDIR $APP_HOME
# Copy only the requirements file to the container
COPY requirements.txt .

# Upgrade pip and install Python dependencies
RUN pip install --upgrade pip && pip install --no-cache-dir -r requirements.txt

# Copy the rest of the application code
COPY . .

# Set the default command to run your application
CMD ["python", "main.py"]
