# Agentes de Validacao

Os agentes usam nomes de Dragon Ball Z como apelidos operacionais. Eles nao mudam a identidade do jogo; servem para lembrar rapidamente quem valida cada dimensao de qualidade.

## Fluxo padrao

1. O agente executor implementa o escopo da sprint.
2. Os agentes de validacao revisam pelos seus criterios.
3. Krillin consolida bugs e regressao.
4. Trunks atualiza estado, decisoes e proximo passo.
5. A sprint so fecha depois da validacao visual no Godot.
6. Commit e push da sprint validada sao obrigatorios antes de iniciar a proxima sprint.

## Goku - Gameplay e Combate

Responsavel por sentir se o jogo esta divertido.

Valida:

- Peso dos golpes.
- Movimento 2.5D.
- Ritmo de combate.
- Knockback, hit stun e resposta visual.
- Se o prototipo funciona mesmo sem arte final.

Documento: `.agents/goku_gameplay.md`

## Vegeta - Arquitetura e Seguranca Tecnica

Responsavel por manter o projeto robusto.

Valida:

- Codigo C# simples e legivel.
- Separacao entre prototipo e sistemas finais.
- Risco de bugs, acoplamento e regressao.
- Uso correto de Godot .NET.
- Build limpo antes de fechar sprint.

Documento: `.agents/vegeta_seguranca_tecnica.md`

## Piccolo - Direcao de Arte

Responsavel pela leitura visual e identidade.

Valida:

- Silhuetas fortes.
- Paleta dark brasileira.
- Clareza da faixa jogavel.
- Composicao de cena.
- Coerencia entre assets gerados e a biblia de arte.

Documento: `.agents/piccolo_direcao_de_arte.md`

## Gohan - Narrativa e Cuidado Cultural

Responsavel por manter o jogo adulto, melancolico e respeitoso.

Valida:

- Tom narrativo.
- Coerencia emocional.
- Evitar caricatura indigena.
- Nomes, simbolos e rituais ficticios com cuidado.
- Violencia com peso dramatico, nao so excesso vazio.

Documento: `.agents/gohan_narrativa_cultural.md`

## Bulma - Ferramentas e CI

Responsavel por Godot, C#, Cursor CI e organizacao tecnica.

Valida:

- Comandos de build.
- Estrutura de pastas.
- Compatibilidade Codex/Cursor.
- Arquivos de configuracao.
- Handoff para outras ferramentas.

Documento: `.agents/bulma_ferramentas_ci.md`

## Trunks - Continuidade e Backlog

Responsavel por lembrar onde paramos.

Valida:

- Sprint atual.
- Decisoes registradas.
- Checklist atualizado.
- Proximo passo claro.
- Handoff entre Codex e Cursor.

Documento: `.agents/trunks_continuidade.md`

## Krillin - QA e Playtest

Responsavel por achar problemas antes que virem divida.

Valida:

- Cenas quebradas.
- Controles basicos.
- Bugs de colisao.
- Regressao de combate.
- Checklist de validacao visual.

Documento: `.agents/krillin_qa.md`
