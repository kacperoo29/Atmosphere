FROM node:lts-bullseye

WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH

COPY . ./
WORKDIR /app/atmosphere
RUN npm install --global --force yarn
RUN yarn install --silent

CMD ["npm", "start"]
