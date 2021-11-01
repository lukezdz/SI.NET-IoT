FROM python:3
WORKDIR /usr/src/app

ENV PYTHONUNBUFFERED=1

COPY requirements.txt ./
COPY publisher.py ./
RUN pip install --no-cache-dir -r requirements.txt

CMD ["python", "/usr/src/app/publisher.py"]
