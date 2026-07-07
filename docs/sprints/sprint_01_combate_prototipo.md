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

- [x] Golpe parece pesado.
- [x] Inimigo reage de forma satisfatoria.
- [x] Leitura de profundidade esta clara.
- [x] Cena tem clima, mesmo com placeholder.

Observacao: validado visualmente no Godot local em 2026-07-07.

## Validacao

Rodar `scenes/Main.tscn` no Godot e gravar decisoes:

- Aprovado.
- Ajustar.
- Cortar.

Status atual: validado localmente no Godot.

## Registro de validacao visual

Data: 2026-07-07.

Ambiente:

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Godot: `4.7.stable.mono.official.5b4e0cb0f`
- .NET: `10.0`

Resultado: aprovado pelo usuario.

Observacoes:

- A primeira tentativa abriu tela cinza porque o Godot nao carregava a assembly do projeto.
- Corrigido `project/assembly_name` de `Terra Sangrada` para `TerraSangrada`.
- `Main.tscn` passou a instanciar `PrototypeArena.tscn` diretamente.
- `PrototypeArena.tscn` agora tem conteudo visivel no editor, nao apenas criado em runtime por C#.

## Agentes envolvidos

- Goku valida sensacao do combate.
- Vegeta valida arquitetura e build.
- Piccolo valida leitura visual dos placeholders e arena.
- Krillin valida regressao jogavel.
- Trunks atualiza estado e proximo passo.
