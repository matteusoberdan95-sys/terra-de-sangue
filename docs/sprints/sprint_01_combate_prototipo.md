# Sprint 1 - Combate Prototipo

## Objetivo

Fazer o boneco andar e bater com peso suficiente para provar o nucleo do jogo.

## Escopo

Entra:

- Movimento 2.5D.
- Ataque leve.
- Inimigo com vida.
- Knockback.
- Camera.
- Arena simples.
- Primeiro passe de hit pause e screen shake.

Nao entra:

- Arte final.
- Boss.
- Fase completa.
- Sistema de inventario.
- Historia longa.

## Checklist tecnico

- [x] Player com estado de idle.
- [x] Player com estado de walk.
- [x] Player com estado de ataque leve.
- [x] Hitbox separada de hurtbox.
- [x] Inimigo recebe dano.
- [x] Inimigo morre.
- [x] Knockback respeita direcao.
- [x] Eixo Y limita area jogavel.
- [x] Camera acompanha sem enjoo.

## Checklist de sensacao

- [ ] Golpe parece pesado.
- [ ] Inimigo reage de forma satisfatoria.
- [ ] Leitura de profundidade esta clara.
- [ ] Cena tem clima, mesmo com placeholder.

Observacao: itens de sensacao dependem de validacao visual no Godot. O build tecnico passou.

## Validacao

Rodar `scenes/Main.tscn` no Godot e gravar decisoes:

- Aprovado.
- Ajustar.
- Cortar.

Status atual: implementado tecnicamente, aguardando validacao visual no Godot.

## Agentes envolvidos

- Goku valida sensacao do combate.
- Vegeta valida arquitetura e build.
- Piccolo valida leitura visual dos placeholders e arena.
- Krillin valida regressao jogavel.
- Trunks atualiza estado e proximo passo.
