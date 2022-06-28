FROM node:lts-bullseye

RUN npm install --global --force yarn
RUN yarn global add --silent react-scripts typescript

WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH

WORKDIR /app/atmosphere
COPY package.json ./
RUN yarn install --silent
RUN chown -R node /app

CMD ["npm", "start"]
