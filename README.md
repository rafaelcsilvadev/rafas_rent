# 🏎️ Rafas Rent - Locadora de Carros (Estudo AWS)

Este projeto é um **sistema extremamente simplificado de uma locadora de carros**, criado com o principal objetivo de **praticar e dominar os serviços da AWS** (Amazon Web Services), integrando-os em uma arquitetura moderna e escalável de microserviços.

## 🚀 Sobre o Projeto

O **Rafas Rent** é uma plataforma que permite simular o aluguel de veículos de forma rápida e eficiente. A ideia principal é aprender "mão na massa" como configurar, desenvolver e gerenciar aplicações reais (como uma locadora) utilizando as melhores práticas do mercado, como C# e Cloud Computing.

### 🛠️ Tecnologias e Decisões

- **Mão no Código + IA:** O desenvolvimento foi feito com implementação manual das regras de negócio, contando com o apoio de Inteligência Artificial como assistente para acelerar e otimizar processos.
- **Arquitetura MVC:** Optei por utilizar o padrão **MVC (Model-View-Controller)** para garantir uma melhor organização do código e separação de responsabilidades.
- **Microserviços:** O projeto será futuramente segmentado em microserviços para praticar o uso de **AWS Lambda** e escalabilidade sem servidor (Serverless).

### ☁️ Serviços AWS Utilizados
- **Amazon Cognito:** Todo o gerenciamento de usuários e segurança é feito através do Cognito, garantindo proteção e autenticação robusta.
- **AWS Lambda (Planejado):** Implementação futura para processos assíncronos e processamento desacoplado.

---

## 🔒 Microserviço de Autenticação (AUTH)

O microserviço **Auth.API** é o responsável por toda a parte de segurança do nosso "clube secreto". Ele gerencia quem pode entrar, quem esqueceu a senha e quem é novo por aqui.

### 🧒 Explicando para uma criança de 10 anos

Imagine que o nosso aplicativo é um **clube secreto** super legal. Para as pessoas brincarem lá dentro, elas precisam ser cadastradas e possuir uma chave.

1. **O Guardião (AWS Cognito):** O nosso serviço de AUTH é como o **Guardião do Clube**. Ele não guarda as senhas em um cofre comum, ele usa um cofre mágico da Amazon chamado Cognito.
2. **Entrando no Clube (Login):** Quando você quer entrar, diz seu nome e sua senha para o Guardião. Se ele conferir e estiver tudo certo, ele te dá uma **pulseirinha mágica** (chamada de Token).
3. **A Pulseirinha Mágica:** Enquanto você estiver com essa pulseirinha, pode passear por todas as salas do clube sem precisar mostrar a senha de novo. mas atenção: a pulseirinha perde a cor depois de um tempo e você precisa pedir uma nova!
4. **Perdi minha Chave! (Esqueci Senha):** Se você esquecer sua senha, o Guardião te manda um código secreto por e-mail para ter certeza que é você mesmo, e só aí ele te deixa criar uma senha nova.
5. **A Recepcionista (MVC):** No código, usamos o padrão MVC. É como ter recepcionistas (Controllers) que recebem seu pedido, olham as fichas de cadastro (Models) e pedem para o Guardião (Repositories) fazer a mágica acontecer.

---

*Este é um projeto em constante evolução para estudos de Nuvem e Desenvolvimento .NET.*
