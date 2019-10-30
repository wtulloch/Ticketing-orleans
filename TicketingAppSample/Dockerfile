FROM node:10-alpine as builder
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run prod

FROM nginx:alpine
COPY nginx/default.conf /etc/nginx/conf.d/
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /dist /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]

